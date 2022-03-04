using System;

namespace Ideine.LogsSender.Interfaces
{
	public interface IContextLogService
	{
		void Log(LogLevel level, Action<IObjectWriter> fillLogEntry);

		void LogRaw(string rawJsonEntry);

		IContextLogService WithAppender(ILogAppender appender);
	}
}