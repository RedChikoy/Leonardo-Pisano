using System;

namespace BLL.Interfaces
{
    /// <summary>
    /// Интерфейс работы с RabbitMQ
    /// </summary>
    public interface IMessageBusService
    {
        void Publish<T>(T message) where T : class;

        void Subscribe<T>(string name, Action<T> handler) where T : class;

        void SendForThread<T>(int threadId, T message) where T : class;

        void ReceiveForThread<T>(int threadId, Action<T> handler) where T : class;
    }
}
