using System.Web.Mvc;

namespace TheAGEnt.Core.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";

            return View();
        }

        public ActionResult AdminPanel()
        {
            return View();
        }
    }
}