using System;
using Ideine.LogsSender.Interfaces;

namespace Ideine.LogsSender.Extensions
{
	public static class ContextLogServiceExtensions
	{
		public static void LogException(this IContextLogService contextLogService, LogLevel logLevel, Exception ex, string message)
		{
			contextLogService.Log(logLevel, x => x
				.WriteException("exception", ex)
				.WriteMethodInfo("context")
				.WriteProperty("message", message));
		}
	}
}
