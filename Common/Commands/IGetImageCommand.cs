using Common.Models;
using System.Threading.Tasks;

namespace Common.Commands
{
    public interface IGetImageCommand
    {
        Task<TResponse> Execute(TRequest req);
    }
}
