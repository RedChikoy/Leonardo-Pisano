using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using BLL.Dto;
using BLL.Interfaces;

namespace BLL.Services
{
    public class ThreadingService: IThreadingService
    {
        private const int SleepTime = 500;
        private const string ContinuerUrl = "http://localhost:63547";
        private const string ApiCaclulateMethod = "api/сaclulate";

        private static readonly HttpClient ContinuerClient = new HttpClient();

        private readonly ICalculationService _calculationService;
        private readonly IMessageBusService _messageBusService;

        public ThreadingService(
            ICalculationService calculationService,
            IMessageBusService messageBusService)
        {
            _calculationService = calculationService;
            _messageBusService = messageBusService;
        }

        public void StartThreads(int count)
        {
            var container = ChislerContainer.GetInstance();

            //var tasks = new List<Task>();
            for (var i = 0; i < count; i++)
            {
                //tasks.Add(Task.Factory.StartNew(() => CalcStarterTask(container.Token)));
                Task.Factory.StartNew(() => CalcStarterTask(container.Token));
            }
        }

        public void StopThreads()
        {
            var container = ChislerContainer.GetInstance();
            container.TokenSource.Cancel();
        }

        public IEnumerable<Chisler> GetCurrentValues()
        {
            var container = ChislerContainer.GetInstance();
            return container.GetCurentCalcValues();
        }

        private void CalcStarterTask(CancellationToken token)
        {
            var threadId = Thread.CurrentThread.ManagedThreadId;
            var container = ChislerContainer.GetInstance();

            //Добавляем подписку на очередь
            _messageBusService.ReceiveForThread<Chisler>(threadId, ReceiveValueFromMq);
            Debug.WriteLine($"Starter {threadId} подписан");

            //Запускаем расчёт c 1
            ProcessStarterCalculationsAsync(threadId, 1).GetAwaiter().GetResult();
            Debug.WriteLine($"Starter {threadId} запущен");

            while (true)
            {
                if (token.IsCancellationRequested)
                {
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
            var newChisler = _calculationService.Calculate(threadId, valueСontinuer);

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
            InitContinuerClient();

            var response = await ContinuerClient.PostAsJsonAsync(ApiCaclulateMethod, newChisler);

            Debug.WriteLine($"Starter {newChisler.ThreadId} отправил значение на три буквы");
        }

        private static void InitContinuerClient()
        {
            ContinuerClient.BaseAddress = new Uri(ContinuerUrl);
            ContinuerClient.DefaultRequestHeaders.Accept.Clear();
            ContinuerClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        private void ProcessСontinuerCalculations(Chisler starterChisler)
        {
            var newChisler = _calculationService.Calculate(starterChisler);

            //Отправка через RabbitMQ
            _messageBusService.SendForThread(newChisler.ThreadId, newChisler);
        }

        /// <summary>
        /// Получить значение из очереди и положить в сумку
        /// </summary>
        /// <param name="value"></param>
        private static void ReceiveValueFromMq(Chisler value)
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
