using System;
using System.Collections.Generic;
using Ideine.LogsSender.Interfaces;

namespace Ideine.LogsSender.Internals
{
	internal class LogService : ILogService
	{
		private readonly ILogSender _sender;
		private readonly LogLevel _minimumLogLevel;
		private readonly List<ILogAppender> _appenders = new List<ILogAppender>();

		public LogService(ILogSender sender, LogLevel minimumLogLevel)
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

			var json = content.ToString();
			var entry = new LogEntry(index, type, json);
			_sender.Enqueue(entry);
		}
	}
}