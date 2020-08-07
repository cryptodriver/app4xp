using Application.Interfaces;
using Common.Models;
using System.Threading.Tasks;

namespace Common.Services
{
    public interface ISampleService : IService
    {
        Task<TResponse> GetDog(TRequest request);
    }
}
