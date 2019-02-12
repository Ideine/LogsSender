using System;

namespace Ideine.LogsSender.Interfaces
{
	public interface IContextLogService
	{
		void Log(LogLevel level, Action<IObjectWriter> fillLogEntry);

		IContextLogService WithAppender(ILogAppender appender);
	}
}