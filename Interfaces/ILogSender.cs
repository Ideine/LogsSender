using System;

namespace Ideine.LogsSender.Interfaces
{
	[Obsolete("Use " + nameof(ILogQueueSender))]
	public interface ILogSender : ILogQueueSender
	{
	}

	public interface ILogQueueSender
	{
		void Enqueue(ILogEntry entry);

		void Enqueue(string rawJsonEntry);
	}
}