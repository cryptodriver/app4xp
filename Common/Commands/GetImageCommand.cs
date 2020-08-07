using Application;
using Application.Core;
using Common.Helpers;
using Common.Models;
using Common.Services;
using System.Threading.Tasks;

namespace Common.Commands
{
    [Autofac(true)]
    public sealed class GetImageCommand : IGetImageCommand
    {
        public async Task<TResponse> Execute(TRequest req)
        {
            LoggerHelper.Write(string.Format("Params: req=[ {0} ]", req?.ToString()));

            var srv = ServiceProvider.Instance.Get<ISampleService>();

            var res = await srv.GetDog(req);

            return res;
        }
    }
}
