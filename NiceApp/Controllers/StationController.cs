using Microsoft.AspNetCore.Mvc;

namespace NiceApp.Controllers
{
    public class StationController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
