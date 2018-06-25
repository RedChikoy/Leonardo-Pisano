using BLL.Dto;
using BLL.Interfaces;
using BLL.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Сontinuer.Tests.Transport
{
    [TestClass]
    public class RabbitTest
    {
        [TestMethod]
        public void SendMessageToRabbit()
        {
            var message = new Chisler { ThreadId = 1, Value = 5 };
            IMessageBusService messageBus = new EasyNetQService();
            ITransportService transportService = new RabbitTransportService(messageBus);

            transportService.Send(message);

            var result = transportService.Get(message.ThreadId);

            Assert.IsNotNull(result);
            Assert.AreEqual(message.Value, result.Value);

            transportService.Close(message.ThreadId);
        }
    }
}
