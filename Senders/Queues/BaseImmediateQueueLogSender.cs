using System.Threading.Tasks;
using Ideine.LogsSender.Interfaces;
using Ideine.LogsSender.Internals;

namespace Ideine.LogsSender.Senders.Queues
{
	public abstract class BaseImmediateQueueLogSender : ILogQueueSender
	{
		private readonly BackgroundQueueWorker<ILogEntry> _worker;

		protected BaseImmediateQueueLogSender()
		{
			_worker = new BackgroundQueueWorker<ILogEntry>(Send);
		}

		public void Enqueue(ILogEntry entry)
		{
			_worker.Queue(entry);
		}

		protected abstract Task Send(ILogEntry entry);
	}
}
