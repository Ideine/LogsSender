using Ideine.LogsSender.Interfaces;

namespace Ideine.LogsSender.Internals
{
	internal class LogEntry : ILogEntry
	{
		public string Index { get; }

		public string Type { get; }

		public string Content { get; }

		public LogEntry(string index, string type, string content)
		{
			Index = index;
			Type = type;
			Content = content;
		}
	}
}