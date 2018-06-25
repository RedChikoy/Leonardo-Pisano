
using System;
using System.Threading.Tasks;
using BLL.Dto;
using BLL.Interfaces;

namespace BLL.Services
{
    public class ApiTransportService : ITransportService
    {
        private static IApiService _apiService;

        public ApiTransportService(IApiService apiService)
        {
            _apiService = apiService;
        }

        public void Send(Chisler value)
        {
            _apiService.SendValueAsync(value);
        }

        public async Task SendAsync(Chisler value)
        {
            await _apiService.SendValueAsync(value);
        }

        public Chisler Get(int queueNumber)
        {
            throw new Exception("Не поддерживается. Получение осуществляется через механизмы ASP.Net WebApi.");
        }

        public void Close(int queueNumber)
        {
            throw new Exception("Не поддерживается. Закрывать не требуется.");
        }
    }
}
