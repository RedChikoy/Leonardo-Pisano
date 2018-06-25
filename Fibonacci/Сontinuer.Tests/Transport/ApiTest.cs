using System;
using BLL.Dto;
using BLL.Interfaces;
using BLL.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Сontinuer.Tests.Transport
{
    [TestClass]
    public class ApiTest
    {
        [TestMethod]
        public void SendMessageToApi()
        {
            var message = new Chisler { ThreadId = 1, Value = 5 };
            IApiService apiBus = new ApiService();
            ITransportService apiService = new ApiTransportService(apiBus);

            var responce = apiService.SendAsync(message);

            //Необходимо указать настройку ContinuerApiUrl и запустить сервис
            Assert.IsNull(responce.Exception);
            Assert.IsTrue(responce.IsCompleted);

            Assert.ThrowsException<Exception>(() => apiService.Get(message.ThreadId));

            IMessageBusService messageBus = new EasyNetQService();
            ITransportService messageService = new RabbitTransportService(messageBus);

            var result = messageService.Get(message.ThreadId);

            Assert.IsNotNull(result);
            Assert.AreEqual(message.Value, result.Value);

            messageService.Close(message.ThreadId);
        }
    }
}
