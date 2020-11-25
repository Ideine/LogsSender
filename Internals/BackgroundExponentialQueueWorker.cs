using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace Ideine.LogsSender.Internals
{
	internal class BackgroundExponentialQueueWorker<TWorkItem> where TWorkItem : class
	{
		private readonly BackgroundWorker _worker;

		private readonly ConcurrentQueue<TWorkItem> _waitingQueue = new ConcurrentQueue<TWorkItem>();
		private readonly SemaphoreSlim _semaphore = new SemaphoreSlim(0, 100000000);

		private readonly Func<TWorkItem, Task<bool>> _itemAction;
		private readonly Action<TWorkItem, Exception> _onException;
		private readonly ExponentialBackOffStrategy _backOffStrategy;

		public BackgroundExponentialQueueWorker(Func<TWorkItem, Task<bool>> itemAction, Action<TWorkItem, Exception> onException = null)
		{
			_itemAction = itemAction;
			_onException = onException;
			_backOffStrategy = new ExponentialBackOffStrategy(5000, 4);

			_worker = new BackgroundWorker(RunAsync);
			_worker.Start();
		}

		public void Queue(TWorkItem item)
		{
			_waitingQueue.Enqueue(item);
			_semaphore.Release();
			_worker.Start();
		}

		private async Task RunAsync()
		{
			TWorkItem item = null;
			int tries = 0;
			while (true)
			{
				await _semaphore.WaitAsync();
				try
				{
					if (_waitingQueue.TryPeek(out item))
					{
						if (await _itemAction.Invoke(item))
						{
							_backOffStrategy.Reset();
							_waitingQueue.TryDequeue(out _);
							tries = 0;
						}
						else
						{
							tries++;
							if (tries > 10) //dismiss when tried sending more than 10 times
							{
								_backOffStrategy.Reset();
								_waitingQueue.TryDequeue(out _);
								tries = 0;
							}
							else
							{
								await _backOffStrategy.Wait();
								_semaphore.Release();
							}
						}
					}
				}
				catch (Exception ex)
				{
					System.Diagnostics.Debug.WriteLine($"Sender exception: {ex}");
					_onException?.Invoke(item, ex);
					await _backOffStrategy.Wait();
				}
			}
		}
	}
}