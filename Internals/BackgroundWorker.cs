using System;
using System.Threading;
using System.Threading.Tasks;

namespace Ideine.LogsSender.Internals
{
	internal class BackgroundWorker
	{
		private readonly object _mutex = new object();
		private readonly Func<Task> _run;
		private Task _currentTask;
		private CancellationTokenSource _cts;

		public BackgroundWorker(Func<Task> run)
		{
			_run = run;
		}

		public void Start()
		{
			if (_currentTask == null || _cts == null || _cts.IsCancellationRequested)
			{
				lock (_mutex)
				{
					if (_currentTask == null || _cts == null || _cts.IsCancellationRequested)
					{
						_cts?.Dispose();
						_cts = new CancellationTokenSource();
						_currentTask = Task.Run(_run, _cts.Token);
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
	}
}