using System.Web.Mvc;
using BLL.Dto;
using BLL.Interfaces;
using BLL.Services;
using Starter.Models;

namespace Starter.Controllers
{
    public class HomeController : Controller
    {
        //TODO Прикрутить DI, если будет время
        private readonly IThreadingService _threadingService;

        public HomeController()
        {
            ICalculationService calculationService = new CalculationService();
            IMessageBusService messageBusService = new EasyNetQService();
            IApiTransportService apiTransportService = new ApiTransportService();

            _threadingService = new ThreadingService(calculationService, messageBusService, apiTransportService);
        }

        public ActionResult Index()
        {
            var model = new StarterModel {IsCalcStarted = false};
            return View(model);
        }

        /// <summary>
        /// Запустить расчёт
        /// </summary>
        /// <returns></returns>
        public ActionResult Start(StarterModel model)
        {
            //Запуск потоков расчёта
            _threadingService.StartThreads(model.ThreadsCount);

            model.IsCalcStarted = true;
            return View("Index", model);
        }

        /// <summary>
        /// Запустить расчёт
        /// </summary>
        /// <returns></returns>
        public ActionResult CheckCurrentResults()
        {
            //Получение результатов из потоков расчёта
            var values = _threadingService.GetCurrentValues(CalcRequestEnum.Starter);

            //TODO Доделать вывод

            var model = new StarterModel { IsCalcStarted = true, CalcValues = values };
            return View("Index", model);
        }

        /// <summary>
        /// Остановить расчёт
        /// </summary>
        /// <returns></returns>
        public ActionResult Stop(StarterModel model)
        {
            //Остановка потоков расчёта
            _threadingService.StopThreads();

            //TODO: сформировать итог вычисления всех потоков и передать через модель

            model.IsCalcStarted = false;
            return View("Index", model);
        }
    }
}
