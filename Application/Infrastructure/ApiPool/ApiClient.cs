using System;
using System.Net.Http;

namespace Application.Infrastructure.ApiPool
{
    public class ApiClient : HttpClient
    {
        public ApiClient() : base()
        {
        }

        public ApiClient(HttpMessageHandler messageHandler) : base(messageHandler)
        {

        }

        public ApiClientState State { get; set; }

        public EventHandler OnDispose;

        protected override void Dispose(bool disposing)
        {
            OnDispose?.Invoke(this, new EventArgs());
        }

        internal void InternalDispose()
        {
            base.Dispose(true);
        }
    }
}
