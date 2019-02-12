namespace Ideine.LogsSender.Interfaces
{
	public interface ILogBufferStorage
	{
		string Load();

		void Save(string content);
	}
}