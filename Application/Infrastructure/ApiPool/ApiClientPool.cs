using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;

namespace Application.Infrastructure.ApiPool
{
    public class ApiClientPool : IApiClientPool
    {
        private readonly List<ApiClient> _pool;

        public int AvailableCount => _pool.Where(x => x.State == ApiClientState.Available).Count();

        public int TotalPoolSize => _pool.Where(x => x.State != ApiClientState.Disposed).Count();

        public ApiClientConfig Configuration { get; set; }

        public Action<HttpClient> ClientInitializationOnCreation;

        public Action<HttpClient> ClientInitializationOnFetch;

        public event EventHandler PoolChanged;

        public ApiClientPool(ApiClientConfig config = null)
        {
            _pool = new List<ApiClient>();

            Configuration = config ?? new ApiClientConfig();
        }

        public ApiClient Fetch()
        {
            lock (_pool)
            {
                var client = _pool.FirstOrDefault(x => x.State == ApiClientState.Available);

                if (client == null)
                {
                    if (Configuration.MessageHandler != null)
                        client = new ApiClient(Configuration.MessageHandler);
                    else
                        client = new ApiClient();

                    ClientInitializationOnCreation?.Invoke(client);

                    client.OnDispose += (sender, e) =>
                    {
                        client.State = ApiClientState.Available;

                        if (Configuration.ResetHeadersOnReuse)
                        {
                            client.DefaultRequestHeaders.Clear();
                        }

                        PoolChanged?.Invoke(this, new EventArgs());
                    };

                    _pool.Add(client);
                }

                if (!String.IsNullOrWhiteSpace(Configuration.BaseUrl))
                {
                    client.BaseAddress = new Uri(Configuration.BaseUrl);
                }

                this.SetAuth(client);

                ClientInitializationOnFetch?.Invoke(client);

                client.State = ApiClientState.InUse;
                PoolChanged?.Invoke(this, new EventArgs());

                return client;
            }
        }

        public void ResetAuth()
        {
            lock (_pool)
            {
                foreach (var client in _pool.Where(x => x.State == ApiClientState.Available).ToList())
                {
                    this.SetAuth(client);
                }

                PoolChanged?.Invoke(this, new EventArgs());
            }
        }

        private void SetAuth(ApiClient client)
        {
            if (Configuration.AuthMethod == AuthMehtod.BASIC)
            {
                if (!String.IsNullOrWhiteSpace(Configuration.Name) && !String.IsNullOrWhiteSpace(Configuration.Password))
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(System.Text.ASCIIEncoding.ASCII.GetBytes($"{Configuration.Name}:{Configuration.Password}")));
                }
            }

            if (Configuration.AuthMethod == AuthMehtod.TOKEN)
            {
                if (!String.IsNullOrWhiteSpace(Configuration.Token))
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Configuration.Token);
                }
            }

            if (Configuration.AuthMethod == AuthMehtod.APIKEY)
            {
                if (!String.IsNullOrWhiteSpace(Configuration.ApiKey))
                {
                    client.DefaultRequestHeaders.Add("api-key", Configuration.ApiKey);
                }
            }
        }

        public void Flush()
        {
            lock (_pool)
            {
                foreach (var client in _pool.Where(x => x.State == ApiClientState.Available).ToList())
                {
                    client.State = ApiClientState.Disposed;
                    client.InternalDispose();
                    _pool.Remove(client);
                }

                PoolChanged?.Invoke(this, new EventArgs());
            }
        }
    }
}
