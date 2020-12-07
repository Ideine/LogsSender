using System.Threading.Tasks;
using Ideine.LogsSender.Interfaces;
using Ideine.LogsSender.Internals;

namespace Ideine.LogsSender.Senders.Queues
{
	public abstract class BaseBackOffQueueLogSender : ILogQueueSender
	{
		private readonly BackgroundExponentialQueueWorker<ILogEntry> _worker;

		protected BaseBackOffQueueLogSender()
		{
			_worker = new BackgroundExponentialQueueWorker<ILogEntry>(Send);
		}

		public void Enqueue(ILogEntry entry)
		{
			_worker.Queue(entry);
		}

		protected abstract Task<bool> Send(ILogEntry entry);
	}
}
