using Application;
using Application.Core;
using Application.Infrastructure.ApiPool;
using Common.Helpers;
using Common.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Dynamic;
using System.Net;
using System.Threading.Tasks;

namespace Common.Services
{
    [Autofac(true)]
    public class SampleService : ISampleService
    {
        public async Task<TResponse> GetDog(TRequest request)
        {
            LoggerHelper.Write();

            var _baseUrl = XSetting.All["API"].app4xp.baseUrl;
            var _action = XSetting.All["API"].app4xp.actions.sample.random;

            var _endpoint = _baseUrl + _action;

            using (var client = ServiceProvider.Instance.Get<IApiClientPool>().Fetch())
            {
                var response = await client.GetAsync($"{_endpoint}");

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    string res = await response.Content.ReadAsStringAsync();
                    dynamic body = JsonConvert.DeserializeObject<ExpandoObject>(res, new ExpandoObjectConverter());

                    return new TResponse()
                    {
                        Code = Status.OK,
                        Body = body
                    };
                }
            }

            return new TResponse() { Code = Status.NG };
        }
    }
}
