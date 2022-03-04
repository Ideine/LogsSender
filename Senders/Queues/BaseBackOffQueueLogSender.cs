using System.Threading.Tasks;
using Ideine.LogsSender.Interfaces;
using Ideine.LogsSender.Internals;

namespace Ideine.LogsSender.Senders.Queues
{
	public abstract class BaseBackOffQueueLogSender : ILogQueueSender
	{
		private readonly BackgroundExponentialQueueWorker<ILogEntry> _worker;

		private readonly BackgroundExponentialQueueWorker<string> _rawWorker;

		protected BaseBackOffQueueLogSender()
		{
			_worker = new BackgroundExponentialQueueWorker<ILogEntry>(Send);
			_rawWorker = new BackgroundExponentialQueueWorker<string>(Send);
		}

		public void Enqueue(ILogEntry entry)
		{
			_worker.Queue(entry);
		}

        public void Enqueue(string rawEntry)
        {
			_rawWorker.Queue(rawEntry);
		}

		protected abstract Task<bool> Send(ILogEntry entry);

		protected abstract Task<bool> Send(string rawEntry);
	}
}
