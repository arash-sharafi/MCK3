using System.Web.Mvc;

namespace Mock3.Areas.Mgt.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ConsoleController : Controller
    {
        // GET: Mgt/Console
        public ActionResult Index()
        {
            return View();
        }
    }
}