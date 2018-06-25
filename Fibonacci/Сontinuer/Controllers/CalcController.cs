using System.Web.Http;
using BLL.Dto;
using BLL.Interfaces;
using BLL.Services;

namespace Сontinuer.Controllers
{
    [RoutePrefix("api")]
    public class CalcController : ApiController
    {
        //TODO Прикрутить DI, если будет время
        private readonly IThreadingService _threadingService;

        public CalcController()
        {
            ICalculationService calculationService = new CalculationService();
            IApiService apiTransportService = new ApiService();
            ITransportService transportService = new ApiTransportService(apiTransportService);

            _threadingService = new ThreadingService(calculationService, transportService);
        }

        [Route("caclulate")]
        [HttpPost]
        public void Caclulate(Chisler value)
        {
            _threadingService.StartСontinuerThread(value);
        }
    }
}
