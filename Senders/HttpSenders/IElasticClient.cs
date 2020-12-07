using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Ideine.LogsSender.Interfaces;

namespace Ideine.LogsSender.Senders.HttpSenders
{
	public interface IHttpSender
	{
		Task<bool> Send(ILogEntry entry);
		Task<bool> Send(IReadOnlyList<ILogEntry> entries);
		Task<bool> Send(string entries);
	}

	public class HttpClientSender : IHttpSender
	{
		private readonly HttpClient _client;
		private readonly string _url;

		public HttpClientSender(HttpClient client, string url)
		{
			_client = client;
			_url = url;
		}

		public Task<bool> Send(ILogEntry entry)
		{
			return Send(new [] { entry });
		}

		public Task<bool> Send(IReadOnlyList<ILogEntry> entries)
		{
			var content = new StringBuilder(entries.Count * (200 + 75));
			foreach (ILogEntry entry in entries)
			{
				content.AppendLine($"{{\"index\":{{\"_index\":\"{entry.Index}\", \"_type\":\"{entry.Type}\"}}");
				content.AppendLine(entry.Content);
			}
			return Send(content.ToString());
		}

		public async Task<bool> Send(string entries)
		{
			try
			{
				using (var stringContent = new StringContent(entries, Encoding.UTF8, "text/plain"))
				using (var result = await _client.PostAsync(_url, stringContent))
				{
					return result.IsSuccessStatusCode;
					//TODO quid du cas où les logs sont mal formés ?
				}
			}
			catch (Exception ex)
			{
				System.Diagnostics.Trace.WriteLine("IDEINE.LOGSSENDER : Exception while sending logs :");
				System.Diagnostics.Trace.WriteLine(ex);
			}

			return false;
		}
	}
}
