using Ideine.LogsSender.Interfaces;

namespace Ideine.LogsSender.Storage
{
	public class InMemoryLogBufferStorage : ILogBufferStorage
	{
		private string _content;

		public string Load() => _content;

		public void Save(string content) => _content = content;
	}
}