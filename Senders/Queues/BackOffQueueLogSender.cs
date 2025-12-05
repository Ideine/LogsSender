using System.Threading.Tasks;
using Ideine.LogsSender.Interfaces;
using Ideine.LogsSender.Senders.HttpSenders;

namespace Ideine.LogsSender.Senders.Queues
{
	public class BackOffQueueLogSender : BaseBackOffQueueLogSender
	{
		private readonly IHttpSender _client;

		public BackOffQueueLogSender(IHttpSender client)
		{
			_client = client;
		}

		protected override Task<bool> Send(ILogEntry entry)
		{
			return _client.Send(entry);
		}

		protected override Task<bool> Send(string rawJsonEntry)
		{
			return _client.Send(rawJsonEntry);
		}

		public override void Flush() { }
	}
}
