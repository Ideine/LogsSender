using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace Ideine.LogsSender.Internals
{
	/// <remarks>
	/// Classe reprise Xmf2.Common.Extensions.
	/// TODO: Publier le package Xmf2.Common.Extensions sur Nuget.org et utiliser le package plutôt que copier cette classe.
	/// </remarks>
	internal class BackgroundQueueWorker<TWorkItem> where TWorkItem : class
	{
		private class BackgroundWorker
		{
			private readonly object _mutex = new object();
			private readonly Func<Task> _run;
			private Task _currentTask;
			private CancellationTokenSource _cts;
			private bool _isRunning;

			public BackgroundWorker(Func<Task> run)
			{
				_run = run;
			}

			public void Start()
			{
				if (_currentTask == null || _cts == null || _cts.IsCancellationRequested || !_isRunning)
				{
					lock (_mutex)
					{
						if (_currentTask == null || _cts == null || _cts.IsCancellationRequested || !_isRunning)
						{
							_cts?.Dispose();
							_cts = new CancellationTokenSource();
							_currentTask = Task.Run(Run, _cts.Token);
						}
					}
				}
			}

			public void Stop()
			{
				lock (_mutex)
				{
					_cts.Cancel();
					_currentTask = null;
				}
			}

			private async Task Run()
			{
				_isRunning = true;
				try
				{
					Debug.WriteLine("BackgroundWorker started");
					await _run();
					Debug.WriteLine("BackgroundWorker stopped");
				}
				catch (Exception ex)
				{
					Debug.WriteLine($"BackgroundWorker stopped with exception : {ex}");
				}
				finally
				{
					_isRunning = false;
				}
			}
		}

		private readonly BackgroundWorker _worker;

		private readonly ConcurrentQueue<TWorkItem> _waitingQueue = new ConcurrentQueue<TWorkItem>();
		private readonly SemaphoreSlim _semaphore = new SemaphoreSlim(0, 100000000);
		private readonly SemaphoreSlim _runAsyncSemaphore = new SemaphoreSlim(1);

		private readonly Func<TWorkItem, Task> _itemAction;
		private readonly Action<TWorkItem, Exception> _onException;

		public BackgroundQueueWorker(Func<TWorkItem, Task> itemAction, Action<TWorkItem, Exception> onException = null)
		{
			_itemAction = itemAction;
			_onException = onException;

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
			while (true)
			{
				await _runAsyncSemaphore.WaitAsync();
				try
				{
					await _semaphore.WaitAsync();

					try
					{
						if (_waitingQueue.TryDequeue(out item) && _itemAction != null)
						{
							await _itemAction.Invoke(item);
						}
					}
					catch (Exception ex)
					{
						_onException?.Invoke(item, ex);
					}
				}
				finally
				{
					_runAsyncSemaphore.Release();
				}
			}
		}
	}
}