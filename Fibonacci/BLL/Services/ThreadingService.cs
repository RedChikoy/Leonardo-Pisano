using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using BLL.Dto;
using BLL.Interfaces;

namespace BLL.Services
{
    public class ThreadingService: IThreadingService
    {
        internal readonly ICalculationService CalculationService;
        internal readonly ITransportService TransportService;

        public ThreadingService(
            ICalculationService calculationService,
            ITransportService transportService)
        {
            CalculationService = calculationService;
            TransportService = transportService;
        }

        public void StartThreads(int count)
        {
            var container = ChislerContainer.GetInstance();

            var tasks = new List<Task>();
            for (var i = 0; i < count; i++)
            {
                tasks.Add(Task.Factory.StartNew(() => CalcTask(this, container.Token)));
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

        private static void CalcTask(ThreadingService tService, CancellationToken token)
        {
            //Флаг инициализации вычисления
            var threadInited = false;

            while (true)
            {
                if (token.IsCancellationRequested)
                {
                    return;
                }

                var threadId = Thread.CurrentThread.ManagedThreadId;
                var container = ChislerContainer.GetInstance();

                //Проверяем очередь Rabbit для текущего потока
                //TODO
                var valueMq = threadInited ? GetMqValue(threadId) : 1;

                //Если в очереди есть значение, то вычисляем и отправляем дальше
                if (valueMq.HasValue)
                {
                    threadInited = true;

                    //Расчёт
                    var currentValue = container.GetCalcValue(threadId);
                    var newValue = tService.CalculationService.Calculate(valueMq.Value, currentValue.Value);

                    //Обновление контейнера результатов
                    container.UpdateCalcValue(threadId, newValue);

                    //Отправка через API
                    //TODO
                }

                Thread.Sleep(500);
            }
        }

        /// <summary>
        /// Получить значение из очереди для потока
        /// </summary>
        /// <param name="threadId"></param>
        /// <returns></returns>
        private static int? GetMqValue(int threadId)
        {
            return 3;
        }
    }
}
