
using System.Threading.Tasks;
using BLL.Dto;
using BLL.Interfaces;

namespace BLL.Services
{
    public class StarterTransportService: ITransportService
    {
        private static IMessageBusService _messageBusService;
        private static IApiService _apiService;

        public StarterTransportService(IMessageBusService messageBusService,
            IApiService apiService)
        {
            _messageBusService = messageBusService;
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
            var message = _messageBusService.AdvancedGet<Chisler>(queueNumber);
            if (message != null && message.MessageAvailable)
            {
                return message.Message.Body;
            }

            return null;
        }

        public void Close(int queueNumber)
        {
            _messageBusService.DeleteQueue(queueNumber);
        }
    }
}
