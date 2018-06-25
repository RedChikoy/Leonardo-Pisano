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
            IMessageBusService messageBus = new EasyNetQService();
            ITransportService continuerService = new ContinuerTransportService(messageBus);

            var responce = continuerService.SendAsync(message);

            //Необходимо указать настройку ContinuerApiUrl и запустить сервис
            Assert.IsNull(responce.Exception);
            Assert.IsTrue(responce.IsCompleted);

            Assert.ThrowsException<Exception>(() => continuerService.Get(message.ThreadId));

            IApiService apiService = new ApiService();
            ITransportService starterService = new StarterTransportService(messageBus, apiService);

            var result = starterService.Get(message.ThreadId);

            Assert.IsNotNull(result);
            Assert.AreEqual(message.Value, result.Value);

            starterService.Close(message.ThreadId);
        }
    }
}
