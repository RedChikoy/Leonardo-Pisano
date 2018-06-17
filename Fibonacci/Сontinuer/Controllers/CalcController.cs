using BLL.Dto;
using System.Web.Mvc;
using BLL.Interfaces;
using BLL.Services;

namespace Сontinuer.Controllers
{
    public class CalcController : Controller
    {
        //TODO Прикрутить DI, если будет время
        private readonly IThreadingService _threadingService;

        public CalcController()
        {
            ICalculationService calculationService = new CalculationService();
            IMessageBusService messageBusService = new EasyNetQService();

            _threadingService = new ThreadingService(calculationService, messageBusService);
        }

        [Route("api/сaclulate")]
        [HttpPost]
        public void Caclulate(Chisler value)
        {
            _threadingService.StartСontinuerThread(value);
        }
    }
}
