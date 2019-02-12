namespace Ideine.LogsSender.Interfaces
{
	public interface ILogSender
	{
		void Enqueue(ILogEntry entry);
	}
}