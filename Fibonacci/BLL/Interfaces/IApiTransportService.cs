
using System.Net.Http;
using System.Threading.Tasks;
using BLL.Dto;

namespace BLL.Interfaces
{
    public interface IApiTransportService
    {
        Task<HttpResponseMessage> SendValueAsync(Chisler value);
    }
}
