using System.Net.Http;
using Ideine.LogsSender.Interfaces;
using Ideine.LogsSender.Senders.HttpSenders;

namespace Ideine.LogsSender.Senders.Queues
{
	public static class QueueLogSenderFactory
	{
		public static ILogQueueSender Create(HttpClient client, string url, ILogBufferStorage storage)
		{
			return new LogQueueSender(new HttpClientSender(client, url), storage);
		}

		public static ILogQueueSender CreateBuffered(HttpClient client, string url, ILogBufferStorage storage, int bufferSize)
		{
			return new BufferedLogSender(new HttpClientSender(client, url), storage, bufferSize);
		}
	}
}
