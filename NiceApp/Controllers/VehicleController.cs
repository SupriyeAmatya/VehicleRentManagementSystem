using Microsoft.AspNetCore.Mvc;
using NiceApp.Services.VehicleServices;

namespace NiceApp.Controllers
{
    public class VehicleController : Controller
    {
        private readonly IVehicleService _vehicleService;

        public VehicleController(IVehicleService vehicleService)
        {
            _vehicleService = vehicleService;
        }
        public IActionResult Index()
        {

            var users = _vehicleService.GetVehicle();
            return View(users);

        }
    }
}
