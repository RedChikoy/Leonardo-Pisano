using System.Web.Mvc;
using First.Models;

namespace Starter.Controllers
{
    public class HomeController : Controller
    {
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
            //TODO: запуск параллельных потоков отправляющих числа на API второго приложения

            model.IsCalcStarted = true;
            return View("Index", model);
        }

        /// <summary>
        /// Остановить расчёт
        /// </summary>
        /// <returns></returns>
        public ActionResult Stop(StarterModel model)
        {
            //Остановка потоков расчёта
            //TODO: остановка всех параллельных потоков вычисления

            //TODO: сформировать итог вычисления всех потоков и передать через модель

            model.IsCalcStarted = false;
            return View("Index", model);
        }
    }
}
