using System;
using System.Collections.Generic;
using Ideine.LogsSender.Interfaces;
using Ideine.LogsSender.LogWriters;

namespace Ideine.LogsSender.Internals
{
	public class LogService : ILogService
	{
		private readonly ILogQueueSender _sender;
		private readonly LogLevel _minimumLogLevel;
		private readonly List<ILogAppender> _appenders = new List<ILogAppender>();

		public LogService(ILogQueueSender sender, LogLevel minimumLogLevel)
		{
			_sender = sender;
			_minimumLogLevel = minimumLogLevel;
		}

		public ILogService WithAppender(ILogAppender appender)
		{
			_appenders.Add(appender);
			return this;
		}

		public IContextLogService CreateContext(string index, string type) => new ContextLogService(this, index, type);

		public void Log(LogLevel level, string index, string type, Action<IObjectWriter> fillLogEntry)
		{
			if (_minimumLogLevel > level)
			{
				return;
			}

			IObjectWriter content = new JsonLogWriter();
			content.WriteObject("Fields", fillLogEntry);
			content.WriteProperty("LogLevel", level.ToString());

			foreach (var appender in _appenders)
			{
				appender.Append(content);
			}

			string json = content.ToString();
			string fields = ((IObjectWriter) new JsonLogWriter()).WriteObject("Fields", fillLogEntry).ToString();
			LogEntry entry = new LogEntry(index, type, json, fields, level);
			_sender.Enqueue(entry);
		}

		public void LogRaw(string rawJsonEntry)
		{
			_sender.Enqueue(rawJsonEntry);
		}
	}
}