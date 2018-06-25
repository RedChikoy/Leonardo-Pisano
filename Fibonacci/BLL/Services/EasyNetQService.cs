using System;
using BLL.Interfaces;
using EasyNetQ;
using EasyNetQ.Topology;

namespace BLL.Services
{
    public class EasyNetQService : IMessageBusService
    {
        private static readonly object SyncRoot = new object();

        private const string BusConn = "host=localhost:5672;virtualhost=/;username=guest;password=guest;timeout=120";
        private const string QueueNamePrefix = "LP.";

        public void Publish<T>(int queueNumber, T message) where T : class
        {
            lock (SyncRoot)
            {
                using (var bus = RabbitHutch.CreateBus(BusConn))
                {
                    bus.Publish(message, queueNumber.ToString());
                }
            }
        }

        public ISubscriptionResult Subscribe<T>(int queueNumber, string subscriptionId, Action<T> handler) where T : class
        {
            lock (SyncRoot)
            {
                using (var bus = RabbitHutch.CreateBus(BusConn))
                {
                    return bus.Subscribe(subscriptionId, handler, x => x.WithTopic(queueNumber.ToString()));
                }
            }
        }

        public void SendForThread<T>(int queueNumber, T message) where T : class
        {
            lock (SyncRoot)
            {
                using (var bus = RabbitHutch.CreateBus(BusConn))
                {
                    bus.Send(GetQueueName(queueNumber), message);
                }
            }
        }

        public IDisposable ReceiveForThread<T>(int queueNumber, Action<T> handler) where T : class
        {
            lock (SyncRoot)
            {
                using (var bus = RabbitHutch.CreateBus(BusConn))
                {
                    return bus.Receive(GetQueueName(queueNumber), handler);
                }
            }
        }

        public void AdvancedPublish<T>(int queueNumber, T message) where T : class
        {
            lock (SyncRoot)
            {
                using (var bus = RabbitHutch.CreateBus(BusConn))
                {
                    var advancedBus = bus.Advanced;
                    var mqMessage = new Message<T>(message);
                    var queueName = GetQueueName(queueNumber);
                    //var prop = new MessageProperties();
                    advancedBus.Publish(Exchange.GetDefault(), queueName, false, mqMessage);
                }
            }
        }

        public IBasicGetResult<T> AdvancedGet<T>(int queueNumber) where T : class
        {
            lock (SyncRoot)
            {
                using (var bus = RabbitHutch.CreateBus(BusConn))
                {
                    var advancedBus = bus.Advanced;
                    var queue = advancedBus.QueueDeclare(GetQueueName(queueNumber));
                    var message = advancedBus.Get<T>(queue);

                    return message;
                }
            }
        }

        public void DeleteQueue(int queueNumber)
        {
            lock (SyncRoot)
            {
                using (var bus = RabbitHutch.CreateBus(BusConn))
                {
                    var advancedBus = bus.Advanced;
                    var queue = advancedBus.QueueDeclare(GetQueueName(queueNumber));
                    advancedBus.QueueDelete(queue);
                }
            }
        }

        private static string GetQueueName(int queueNumber)
        {
            return $"{QueueNamePrefix}{queueNumber}";
        }
    }
}
