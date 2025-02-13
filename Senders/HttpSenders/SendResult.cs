using System.Net;

namespace Ideine.LogsSender.Senders.HttpSenders
{
    public class SendResult
    {
        public bool IsSuccess { get; set; }
        public HttpStatusCode StatusCode { get; set; }
    }
}