using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Ideine.LogsSender.Extensions;
using Ideine.LogsSender.Interfaces;
using Ideine.LogsSender.Internals;
using Ideine.LogsSender.Senders.HttpSenders;

namespace Ideine.LogsSender.Senders.Queues
{
	internal class BufferedLogSender : ILogQueueSender
	{
		private readonly IHttpSender _sender;
		private readonly ILogBufferStorage _storage;
		private readonly int _bufferSize;

		private readonly BackgroundWorker _worker;

		/// <summary>
		/// Buffer des logs
		/// </summary>
		private readonly StringBuilder _waitingContent = new StringBuilder();

		/// <summary>
		/// Rend synchrone toutes les modifications du buffer de log
		/// </summary>
		private readonly SemaphoreSlim _stringBuilderMutex = new SemaphoreSlim(initialCount: 1);

		/// <summary>
		/// Bloque l'envoie des logs s'il n'y a rien à envoyer
		/// </summary>
		//maxCount est très grand pour ne pas être atteint, sinon on risque un crash
		//TODO extension qui release seulement si on est pas au maxCount
		private readonly SemaphoreSlim _contentSemaphore = new SemaphoreSlim(initialCount: 0, maxCount: 100_000_000);

		/// <summary>
		/// Limite l'execution de l'envoie des logs à une seule task
		/// </summary>
		private readonly SemaphoreSlim _runAsyncSemaphore = new SemaphoreSlim(initialCount: 1);

		private readonly ExponentialBackOffStrategy _backOffStrategy;

		public BufferedLogSender(IHttpSender sender, ILogBufferStorage storage, int bufferSize)
		{
			_sender = sender;
			_storage = storage;
			_bufferSize = bufferSize;

			_backOffStrategy = new ExponentialBackOffStrategy(5000, 4);

			string storedContent = _storage.Load();

			//on lance l'envoie de logs seulement s'il y a quelque chose à envoyer
			if (!string.IsNullOrEmpty(storedContent))
			{
				_waitingContent.Append(storedContent);
				_contentSemaphore.Release();
			}

			_worker = new BackgroundWorker(RunAsync);
			_worker.Start();
		}

		private async Task RunAsync()
		{
			while (true)
			{
				using (await _runAsyncSemaphore.LockAsync())
				{
					await _contentSemaphore.WaitAsync();

					string contentToSend;
					using (await _stringBuilderMutex.LockAsync())
					{
						contentToSend = _waitingContent.ToString();
					}

					if (await _sender.Send(contentToSend))
					{
						using (await _stringBuilderMutex.LockAsync())
						{
							_waitingContent.Remove(0, contentToSend.Length);
							StoreBuffer(_waitingContent.ToString());
							if (_waitingContent.Length != 0)
							{
								_contentSemaphore.Release();
							}
						}

						_backOffStrategy.Reset();
					}
					else
					{
						await _backOffStrategy.Wait();
						_contentSemaphore.Release();
					}
				}
			}
		}

		public async void Enqueue(ILogEntry entry)
		{
			int count;
			using (await _stringBuilderMutex.LockAsync())
			{
				_waitingContent.AppendLine($"{{\"index\":{{\"_index\":\"{entry.Index}\", \"_type\":\"{entry.Type}\"}}}}")
					.AppendLine(entry.Content);

				StoreBuffer(_waitingContent.ToString());

				count = _waitingContent.NumberOfLines() / 2;
			}

			if (count >= _bufferSize)
			{
				_contentSemaphore.Release();
			}

			_worker.Start();
		}

		public async void Enqueue(string rawJsonEntry)
		{
			int count;
			using (await _stringBuilderMutex.LockAsync())
			{
				_waitingContent.AppendLine(rawJsonEntry);

				StoreBuffer(_waitingContent.ToString());

				count = _waitingContent.NumberOfLines() / 2;
			}

			if (count >= _bufferSize)
			{
				_contentSemaphore.Release();
			}

			_worker.Start();
		}

		public void Flush()
		{
			_contentSemaphore.Release();
			_worker.Start();
		}

		private void StoreBuffer(string bufferContent)
		{
			_storage.Save(bufferContent);
		}
	}
}