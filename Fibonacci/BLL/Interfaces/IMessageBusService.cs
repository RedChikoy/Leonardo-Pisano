using System;
using EasyNetQ;

namespace BLL.Interfaces
{
    /// <summary>
    /// Интерфейс работы с RabbitMQ
    /// </summary>
    public interface IMessageBusService
    {
        void Publish<T>(int threadId, T message) where T : class;

        ISubscriptionResult Subscribe<T>(int threadId, string name, Action<T> handler) where T : class;

        void SendForThread<T>(int threadId, T message) where T : class;

        IDisposable ReceiveForThread<T>(int threadId, Action<T> handler) where T : class;

        void AdvancedPublish<T>(int threadId, T message) where T : class;

        IBasicGetResult<T> AdvancedGet<T>(int threadId) where T : class;
    }
}
