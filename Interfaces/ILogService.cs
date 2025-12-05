using System;

namespace Ideine.LogsSender.Interfaces
{
	public interface ILogService
	{
		void Log(LogLevel level, string index, string type, Action<IObjectWriter> fillLogEntry);

		void LogRaw(string rawJsonEntry);

		void Flush();

		ILogService WithAppender(ILogAppender appender);

		IContextLogService CreateContext(string index, string type);
	}
}