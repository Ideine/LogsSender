using System;
using System.Threading;
using System.Threading.Tasks;

namespace Ideine.LogsSender.Extensions
{
	public static class SemaphoreExtensions
	{
		private class SemaphoreLockDisposable : IDisposable
		{
			private SemaphoreSlim _mutex;

			public SemaphoreLockDisposable(SemaphoreSlim mutex)
			{
				_mutex = mutex;
			}

			public void Dispose()
			{
				_mutex?.Release();
				_mutex = null;
			}
		}

		public static async Task<IDisposable> LockAsync(this SemaphoreSlim mutex)
		{
			await mutex.WaitAsync();
			return new SemaphoreLockDisposable(mutex);
		}

		public static IDisposable Lock(this SemaphoreSlim mutex)
		{
			mutex.Wait();
			return new SemaphoreLockDisposable(mutex);
		}
	}
}