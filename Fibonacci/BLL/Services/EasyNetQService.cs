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

        public void Publish<T>(int threadId, T message) where T : class
        {
            lock (SyncRoot)
            {
                using (var bus = RabbitHutch.CreateBus(BusConn))
                {
                    bus.Publish(message, threadId.ToString());
                }
            }
        }

        public ISubscriptionResult Subscribe<T>(int threadId, string subscriptionId, Action<T> handler) where T : class
        {
            lock (SyncRoot)
            {
                using (var bus = RabbitHutch.CreateBus(BusConn))
                {
                    return bus.Subscribe(subscriptionId, handler, x => x.WithTopic(threadId.ToString()));
                }
            }
        }

        public void SendForThread<T>(int threadId, T message) where T : class
        {
            lock (SyncRoot)
            {
                using (var bus = RabbitHutch.CreateBus(BusConn))
                {
                    bus.Send(GetQueueName(threadId), message);
                }
            }
        }

        public IDisposable ReceiveForThread<T>(int threadId, Action<T> handler) where T : class
        {
            lock (SyncRoot)
            {
                using (var bus = RabbitHutch.CreateBus(BusConn))
                {
                    return bus.Receive(GetQueueName(threadId), handler);
                }
            }
        }

        public void AdvancedPublish<T>(int threadId, T message) where T : class
        {
            lock (SyncRoot)
            {
                using (var bus = RabbitHutch.CreateBus(BusConn))
                {
                    var advancedBus = bus.Advanced;
                    var mqMessage = new Message<T>(message);
                    var queueName = GetQueueName(threadId);
                    //var prop = new MessageProperties();
                    advancedBus.Publish(Exchange.GetDefault(), queueName, false, mqMessage);
                }
            }
        }

        public IBasicGetResult<T> AdvancedGet<T>(int threadId) where T : class
        {
            lock (SyncRoot)
            {
                using (var bus = RabbitHutch.CreateBus(BusConn))
                {
                    var advancedBus = bus.Advanced;
                    var queue = advancedBus.QueueDeclare(GetQueueName(threadId));
                    var message = advancedBus.Get<T>(queue);

                    return message;
                }
            }
        }

        private static string GetQueueName(int threadId)
        {
            return $"{QueueNamePrefix}{threadId}";
        }
    }
}
