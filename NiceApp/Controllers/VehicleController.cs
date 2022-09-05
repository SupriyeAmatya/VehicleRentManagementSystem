using Microsoft.AspNetCore.Mvc;
using NiceApp.Models.DataModel;
using NiceApp.Models.DTO;
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
        [HttpGet]
        public IActionResult Create()
        {
            return View(new VehicleDTO());
        }
        [HttpPost]
        public IActionResult Create(VehicleDTO person)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _vehicleService.AddVehicleAsync(person);
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
            }
            return View(person);
        }
    }
}
