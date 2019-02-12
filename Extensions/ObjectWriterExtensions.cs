using System;
using System.Runtime.CompilerServices;
using Ideine.LogsSender.Interfaces;

namespace Ideine.LogsSender.Extensions
{
	public static class ObjectWriterExtensions
	{
		public static IObjectWriter WriteException(this IObjectWriter writer, string property, Exception exception)
		{
			if (exception == null)
			{
				return writer;
			}
			else
			{
				return writer.WriteObject(property, x => InternalWriteException(x, exception));
			}
		}

		private static void InternalWriteException(IObjectWriter writer, Exception exception)
		{
			writer.WriteProperty("message", exception.Message).WriteProperty("stacktrace", exception.StackTrace);

			if (exception is AggregateException aggregateException)
			{
				writer.WriteArray("inners", array =>
				{
					foreach (var innerException in aggregateException.InnerExceptions)
					{
						array.WriteObject(x => InternalWriteException(x, innerException));
					}
				});
			}
			else if (exception.InnerException != null)
			{
				writer.WriteException("inner", exception.InnerException);
			}
		}

		public static IObjectWriter WriteMethodInfo(this IObjectWriter writer, string property, [CallerFilePath] string file = null, [CallerLineNumber] int line = 0, [CallerMemberName] string member = null)
		{
			return writer.WriteObject(property, x => x.WriteProperty("file", file).WriteProperty("line", line).WriteProperty("member", member));
		}
	}
}