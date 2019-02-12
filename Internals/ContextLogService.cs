using System;
using Ideine.LogsSender.Interfaces;

namespace Ideine.LogsSender.Internals
{
	internal class ContextLogService : IContextLogService
	{
		private readonly ILogService _service;
		private readonly string _index;
		private readonly string _type;

		internal ContextLogService(ILogService service, string index, string type)
		{
			_service = service;
			_index = index;
			_type = type;
		}

		public void Log(LogLevel level, Action<IObjectWriter> fillLogEntry)
		{
			_service.Log(level, _index, _type, fillLogEntry);
		}

		public IContextLogService WithAppender(ILogAppender appender)
		{
			_service.WithAppender(appender);
			return this;
		}
	}
}