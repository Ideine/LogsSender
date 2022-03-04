using System.Collections.Generic;
using System.Threading.Tasks;
using Ideine.LogsSender.Interfaces;
using Ideine.LogsSender.Internals;

namespace Ideine.LogsSender.Senders.Queues
{
	public abstract class BaseBufferedBackOffQueueLogSender : ILogQueueSender
	{
		private readonly BufferedBackgroundExponentialQueueWorker<ILogEntry> _worker;
		private readonly BufferedBackgroundExponentialQueueWorker<string> _rawWorker;

		protected BaseBufferedBackOffQueueLogSender()
		{
			_worker = new BufferedBackgroundExponentialQueueWorker<ILogEntry>(Send);
			_rawWorker = new BufferedBackgroundExponentialQueueWorker<string>(Send);
		}

		public void Enqueue(ILogEntry entry)
		{
			_worker.Queue(entry);
		}

        public void Enqueue(string rawJsonEntry)
        {
			_rawWorker.Queue(rawJsonEntry);
		}

		protected abstract Task<bool> Send(IReadOnlyList<ILogEntry> entries);
		protected abstract Task<bool> Send(IReadOnlyList<string> rawEntries);
	}
}
