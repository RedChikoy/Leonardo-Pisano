using System.Web.Mvc;
using Сontinuer.Models;

namespace Сontinuer.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var model = new СontinuerModel { IsCalcStarted = false };
            return View(model);
        }
    }
}
