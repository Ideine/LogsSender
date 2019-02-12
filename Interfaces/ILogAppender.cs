namespace Ideine.LogsSender.Interfaces
{
	public interface ILogAppender
	{
		void Append(IObjectWriter logEntry);
	}
}