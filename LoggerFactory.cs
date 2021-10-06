using System.Net.Http;
using Ideine.LogsSender.Appenders;
using Ideine.LogsSender.Interfaces;
using Ideine.LogsSender.Internals;
using Ideine.LogsSender.Senders.Queues;

namespace Ideine.LogsSender
{
	public static class LoggerFactory
	{
		public static IContextLogService Create(string index, string type, LogLevel minimumLogLevel, HttpClient client, string url, ILogBufferStorage storage)
		{
			return CreateCustom(index, type, minimumLogLevel, sender: QueueLogSenderFactory.Create(client, url, storage));
		}

		public static IContextLogService CreateBuffered(string index, string type, LogLevel minimumLogLevel, HttpClient client, string url, ILogBufferStorage storage, int bufferSize)
		{
			return CreateCustom(index, type, minimumLogLevel, sender: QueueLogSenderFactory.CreateBuffered(client, url, storage, bufferSize));
		}

		public static IContextLogService CreateCustom(string index, string type, LogLevel minimumLogLevel, ILogQueueSender sender)
		{
			return CreateCustom(index, type, new LogService(sender, minimumLogLevel));
		}

		internal static IContextLogService CreateCustom(string index, string type, ILogService logService)
		{
			return new ContextLogService(logService, index, type).WithAppender(new TimestampLogAppender());
		}
	}
}