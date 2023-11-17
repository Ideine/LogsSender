using Ideine.LogsSender.Interfaces;

namespace Ideine.LogsSender.Internals
{
	internal class LogEntry : ILogEntry
	{
		public string Index { get; }

		public string Type { get; }

		public string Content { get; }

		public string Fields { get; }

		public LogLevel Level { get; }

		public LogEntry(string index, string type, string content, string fields, LogLevel level)
		{
			Index = index;
			Type = type;
			Content = content;
			Fields = fields;
			Level = level;
		}
	}
}