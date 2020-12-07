using System.Collections.Generic;
using System.Threading.Tasks;
using Ideine.LogsSender.Interfaces;
using Ideine.LogsSender.Internals;

namespace Ideine.LogsSender.Senders.Queues
{
	public abstract class BaseBufferedBackOffQueueLogSender : ILogQueueSender
	{
		private readonly BufferedBackgroundExponentialQueueWorker<ILogEntry> _worker;

		protected BaseBufferedBackOffQueueLogSender()
		{
			_worker = new BufferedBackgroundExponentialQueueWorker<ILogEntry>(Send);
		}

		public void Enqueue(ILogEntry entry)
		{
			_worker.Queue(entry);
		}

		protected abstract Task<bool> Send(IReadOnlyList<ILogEntry> entries);
	}
}
