using System.Net.Http;
using Ideine.LogsSender.Appenders;
using Ideine.LogsSender.Interfaces;
using Ideine.LogsSender.Internals;
using Ideine.LogsSender.Senders;

namespace Ideine.LogsSender
{
	public static class LoggerFactory
	{
		public static IContextLogService Create(string index, string type, LogLevel minimumLogLevel, HttpClient client, string url, ILogBufferStorage storage)
		{
			return new ContextLogService(
				new LogService(
					new LogSender(client, url, storage),
					minimumLogLevel
				),
				index,
				type
			).WithAppender(new TimestampLogAppender());
		}

		public static IContextLogService CreateBuffered(string index, string type, LogLevel minimumLogLevel, HttpClient client, string url, ILogBufferStorage storage, int bufferSize)
		{
			return new ContextLogService(
				new LogService(
					new BufferedLogSender(client, url, storage, bufferSize),
					minimumLogLevel
				),
				index,
				type
			).WithAppender(new TimestampLogAppender());
		}

		public static IContextLogService CreateCustom(string index, string type, LogLevel minimumLogLevel, ILogSender sender)
		{
			return new ContextLogService(
				new LogService(sender, minimumLogLevel),
				index,
				type
			).WithAppender(new TimestampLogAppender());
		}
	}
}