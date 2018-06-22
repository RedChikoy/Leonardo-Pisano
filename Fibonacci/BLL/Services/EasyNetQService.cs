﻿using System;
using BLL.Interfaces;
using EasyNetQ;

namespace BLL.Services
{
    public class EasyNetQService : IMessageBusService
    {
        private static readonly object SyncRoot = new object();

        private const string BusConn = "host=localhost:5672;virtualhost=/;username=guest;password=guest";
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

        private static string GetQueueName(int threadId)
        {
            return $"{QueueNamePrefix}{threadId}";
        }
    }
}
