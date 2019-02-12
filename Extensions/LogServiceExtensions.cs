using System;
using Ideine.LogsSender.Interfaces;

namespace Ideine.LogsSender.Extensions
{
	public static class LogServiceExtensions
	{
		public static void Debug(this IContextLogService service, Action<IObjectWriter> fillLogEntry)
		{
			service.Log(LogLevel.Debug, fillLogEntry);
		}

		public static void Trace(this IContextLogService service, Action<IObjectWriter> fillLogEntry)
		{
			service.Log(LogLevel.Trace, fillLogEntry);
		}

		public static void Information(this IContextLogService service, Action<IObjectWriter> fillLogEntry)
		{
			service.Log(LogLevel.Information, fillLogEntry);
		}

		public static void Warning(this IContextLogService service, Action<IObjectWriter> fillLogEntry)
		{
			service.Log(LogLevel.Warning, fillLogEntry);
		}

		public static void Error(this IContextLogService service, Action<IObjectWriter> fillLogEntry)
		{
			service.Log(LogLevel.Error, fillLogEntry);
		}

		public static void Critical(this IContextLogService service, Action<IObjectWriter> fillLogEntry)
		{
			service.Log(LogLevel.Critical, fillLogEntry);
		}

		public static void Debug(this IContextLogService service, string message)
		{
			service.Log(LogLevel.Debug, x => x.WriteProperty("message", message));
		}

		public static void Trace(this IContextLogService service, string message)
		{
			service.Log(LogLevel.Trace, x => x.WriteProperty("message", message));
		}

		public static void Information(this IContextLogService service, string message)
		{
			service.Log(LogLevel.Information, x => x.WriteProperty("message", message));
		}

		public static void Warning(this IContextLogService service, string message)
		{
			service.Log(LogLevel.Warning, x => x.WriteProperty("message", message));
		}

		public static void Error(this IContextLogService service, string message)
		{
			service.Log(LogLevel.Error, x => x.WriteProperty("message", message));
		}

		public static void Critical(this IContextLogService service, string message)
		{
			service.Log(LogLevel.Critical, x => x.WriteProperty("message", message));
		}
	}
}