using System;
using EasyNetQ;

namespace BLL.Interfaces
{
    /// <summary>
    /// Интерфейс работы с RabbitMQ
    /// </summary>
    public interface IMessageBusService
    {
        void Publish<T>(int queueNumber, T message) where T : class;

        ISubscriptionResult Subscribe<T>(int queueNumber, string name, Action<T> handler) where T : class;

        void SendForThread<T>(int queueNumber, T message) where T : class;

        IDisposable ReceiveForThread<T>(int queueNumber, Action<T> handler) where T : class;

        void AdvancedPublish<T>(int queueNumber, T message) where T : class;

        IBasicGetResult<T> AdvancedGet<T>(int queueNumber) where T : class;

        void DeleteQueue(int queueNumber);
    }
}
