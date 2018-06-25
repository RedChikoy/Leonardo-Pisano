
using System;
using System.Threading.Tasks;
using BLL.Dto;
using BLL.Interfaces;

namespace BLL.Services
{
    public class ContinuerTransportService : ITransportService
    {
        private static IMessageBusService _messageBusService;

        public ContinuerTransportService(IMessageBusService messageBusService)
        {
            _messageBusService = messageBusService;
        }

        public void Send(Chisler value)
        {
            _messageBusService.SendForThread(value.ThreadId, value);
        }

        public async Task SendAsync(Chisler value)
        {
            throw new NotImplementedException();
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
