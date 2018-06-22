using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using BLL.Dto;
using BLL.Interfaces;

namespace BLL.Services
{
    public class ThreadingService: IThreadingService
    {
        private const int SleepTime = 5;

        private readonly ICalculationService _calculationService;
        private readonly IMessageBusService _messageBusService;
        private readonly IApiTransportService _apiTransportService;

        public ThreadingService(
            ICalculationService calculationService,
            IMessageBusService messageBusService,
            IApiTransportService apiTransportService)
        {
            _calculationService = calculationService;
            _messageBusService = messageBusService;
            _apiTransportService = apiTransportService;
        }

        public void StartThreads(int count)
        {
            var container = ChislerContainer.GetInstance();
            container.Reset();

            for (var i = 0; i < count; i++)
            {
                Task.Factory.StartNew(() => CalcTasksStarter(container.Token));
            }
        }

        public void StopThreads()
        {
            var container = ChislerContainer.GetInstance();
            container.TokenSource.Cancel();
        }

        public IEnumerable<Chisler> GetCurrentValues(CalcRequestEnum сalcRequest)
        {
            var container = ChislerContainer.GetInstance();
            return container.GetCurentCalcValues(сalcRequest);
        }

        private void CalcTasksStarter(CancellationToken token)
        {
            var threadId = Thread.CurrentThread.ManagedThreadId;
            var container = ChislerContainer.GetInstance();

            //Добавляем подписку на очередь
            var receiveKiller = _messageBusService.ReceiveForThread<Chisler>(threadId, ReceiveValueFromMq);
            //var receiveKiller = 
            //    _messageBusService.Subscribe<Chisler>(threadId, $"Subs:{threadId}", ReceiveValueFromMq);
            Debug.WriteLine($"Starter {threadId} подписан");

            //Запускаем расчёт c 1
            ProcessStarterCalculationsAsync(threadId, 1).GetAwaiter().GetResult();
            Debug.WriteLine($"Starter {threadId} запущен");

            while (true)
            {
                if (token.IsCancellationRequested)
                {
                    receiveKiller.Dispose();

                    //TODO Добавить удаление очереди потока
                    //_messageBusService.

                    return;
                }

                //Проверяем сумку для текущего потока
                var valueMq = container.GetFromBug(threadId);

                //Если в очереди есть значение, то вычисляем и отправляем дальше
                if (valueMq != null)
                {
                    //Запускаем расчёт
                    ProcessStarterCalculationsAsync(threadId, valueMq.Value).GetAwaiter();

                    Debug.WriteLine($"Starter {threadId} вычислил");
                }
                else
                {
                    receiveKiller = _messageBusService.ReceiveForThread<Chisler>(threadId, ReceiveValueFromMq);
                    //receiveKiller =
                    //    _messageBusService.Subscribe<Chisler>(threadId, $"Subs:{threadId}", ReceiveValueFromMq);
                    Debug.WriteLine($"Starter {threadId} ждёт");
                }

                //Для наглядности
                // ReSharper disable once ConditionIsAlwaysTrueOrFalse
                if (SleepTime > 0)
                {
                    Thread.Sleep(SleepTime);
                }
            }
        }

        private async Task ProcessStarterCalculationsAsync(int threadId, int valueСontinuer)
        {
            var newChisler = _calculationService.Calculate(threadId, valueСontinuer, CalcRequestEnum.Starter);

            //Отправка через API
            await SendToApiAsync(newChisler);
        }

        //TODO: разделить на разные классы (или даже библиотеки) логику для Starter и  для Continuer
        /// <summary>
        /// Отправить значение на API Continuer
        /// </summary>
        /// <param name="newChisler"></param>
        private async Task SendToApiAsync(Chisler newChisler)
        {
            var result = await _apiTransportService.SendValueAsync(newChisler);

            Debug.WriteLine($"Starter {newChisler.ThreadId} отправил значение. StatusCode: {result.StatusCode}");
        }

        private void ProcessСontinuerCalculations(Chisler starterChisler)
        {
            var newChisler = _calculationService.Calculate(starterChisler, CalcRequestEnum.Continuer);

            //Отправка через RabbitMQ
            _messageBusService.SendForThread(newChisler.ThreadId, newChisler);
            //_messageBusService.Publish(newChisler.ThreadId, newChisler);
        }

        /// <summary>
        /// Получить значение из очереди и положить в сумку
        /// </summary>
        /// <param name="value"></param>
        public void ReceiveValueFromMq(Chisler value)
        {
            var container = ChislerContainer.GetInstance();
            container.PutToBug(value);
        }

        /// <summary>
        /// Запустить вычисление Сontinuer
        /// </summary>
        /// <param name="value"></param>
        public void StartСontinuerThread(Chisler value)
        {
            Task.Factory.StartNew(() => ProcessСontinuerCalculations(value));
        }
    }
}
