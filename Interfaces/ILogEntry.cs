namespace Ideine.LogsSender.Interfaces
{
	public interface ILogEntry
	{
		string Index { get; }
		string Type { get; }
		string Content { get; }
	}
}