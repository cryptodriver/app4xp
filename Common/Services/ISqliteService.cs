using Application.Interfaces;
using Common.Models;
using System.Threading.Tasks;

namespace Common.Services
{
    public interface ISqliteService : IService
    {
        Task<TResponse> UpdateSetting(TRequest request);
    }
}
