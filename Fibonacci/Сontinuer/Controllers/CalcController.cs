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

        public CalcController(IThreadingService threadingService)
        {
            _threadingService = threadingService;
        }

        [Route("caclulate")]
        [HttpPost]
        public void Caclulate(Chisler value)
        {
            _threadingService.StartСontinuerThread(value);
        }
    }
}
