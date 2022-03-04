using System.Threading.Tasks;
using Ideine.LogsSender.Interfaces;
using Ideine.LogsSender.Internals;

namespace Ideine.LogsSender.Senders.Queues
{
	public abstract class BaseImmediateQueueLogSender : ILogQueueSender
	{
		private readonly BackgroundQueueWorker<ILogEntry> _worker;
		private readonly BackgroundQueueWorker<string> _rawWorker;

		protected BaseImmediateQueueLogSender()
		{
			_worker = new BackgroundQueueWorker<ILogEntry>(Send);
			_rawWorker = new BackgroundQueueWorker<string>(Send);
		}

		public void Enqueue(ILogEntry entry)
		{
			_worker.Queue(entry);
		}

        public void Enqueue(string rawJsonEntry)
        {
			_rawWorker.Queue(rawJsonEntry);
		}

		protected abstract Task Send(ILogEntry entry);
		protected abstract Task Send(string entry);
	}
}
