using BLL.Dto;
using BLL.Interfaces;

namespace BLL.Services
{
    public class TransportSevice: ITransportService
    {
        public Chisler GetValueMq(int threadId)
        {
            //TODO: получить значение из Rabbit
            var result = new Chisler
            {
                ThreadId = threadId,
                Value = 1
            };

            return result;
        }

        public void SendValueMq(Chisler value)
        {
            //TODO: отправить значение через Rabbit
        }

        public void SentValueApi(Chisler value)
        {
            //TODO: отправить значение через API
        }
    }
}
