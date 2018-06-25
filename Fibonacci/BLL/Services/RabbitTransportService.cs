
using BLL.Dto;
using BLL.Interfaces;

namespace BLL.Services
{
    public class RabbitTransportService: ITransportService
    {
        private static IMessageBusService _messageBusService;

        public RabbitTransportService(IMessageBusService messageBusService)
        {
            _messageBusService = messageBusService;
        }

        public void Send(Chisler value)
        {
            _messageBusService.SendForThread(value.ThreadId, value);
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
    }
}
