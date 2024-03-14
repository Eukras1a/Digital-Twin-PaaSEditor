using System.Threading.Tasks;
using Utils.Service;

namespace Networks
{
    public interface IServiceProxy
    {
        Task<ServiceResult<TResponse>>  TryRequestAsync<TRequest, TResponse>(string api, TRequest request);
        TResponse Request<TRequest, TResponse>(string api, TRequest request);
    }
}