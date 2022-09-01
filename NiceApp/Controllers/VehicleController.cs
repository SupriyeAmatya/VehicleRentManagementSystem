using Microsoft.AspNetCore.Mvc;

namespace NiceApp.Controllers
{
    public class VehicleController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
