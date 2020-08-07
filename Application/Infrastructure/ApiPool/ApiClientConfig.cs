using System.Net.Http;

namespace Application.Infrastructure.ApiPool
{
    public enum AuthMehtod
    {
        BASIC,
        TOKEN,
        APIKEY
    }

    public class ApiClientConfig
    {
        public string BaseUrl { get; set; }
        public bool ResetHeadersOnReuse { get; set; }
        public HttpMessageHandler MessageHandler { get; set; }

        public AuthMehtod AuthMethod { get; set; } = AuthMehtod.BASIC;

        public string Name { get; set; }
        public string Password { get; set; }

        public string Token { get; set; }

        public string ApiKey { get; set; }
    }
}
