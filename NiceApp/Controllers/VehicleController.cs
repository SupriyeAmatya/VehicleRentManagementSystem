using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using NiceApp.Data;
using NiceApp.Models.DataModel;
using NiceApp.Models.DTO;
using NiceApp.Services.UserhomeService;
using NiceApp.Services.VehicleServices;

namespace NiceApp.Controllers
{
    public class VehicleController : Controller
    {
        private readonly IVehicleService _vehicleService;
        private readonly IUserhomeService _UserhomeService;

        public VehicleController(IVehicleService vehicleService, AppDbContext db, IUserhomeService userhomeService)
        {
            _vehicleService = vehicleService;
            _UserhomeService = userhomeService;
        }
        public IActionResult Index()
        {

            var users = _vehicleService.GetVehicle();



            return View(users);

        }
        [HttpGet]
        public IActionResult Create()
        {

            var mStationList = _vehicleService.GetAllStation();

            ViewBag.AllVehiclesListStation = mStationList.ToList();
            return View(new VehicleDTO());
        }
        [HttpPost]
        public async Task<IActionResult> Create(VehicleDTO person, int station)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    ViewBag.AllVehiclesListStation = new SelectList(_vehicleService.GetAllStation());

                    await _vehicleService.AddVehicleAsync(person);
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
            }
            return View(person);
        }
        public IActionResult Details(int id)
        {
            var model = _vehicleService.CompleteDetails(id);
            return View(model);
        }
        public ActionResult Delete(int id, bool? saveChangesError)
        {
            if (saveChangesError.GetValueOrDefault())
            {
                ViewBag.ErrorMessage = "Unable to save changes. Try again, and if the problem persists see your system administrator.";
            }
            Vehicle user = _vehicleService.GetVehicleById(id);
            return View(user);
        }
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                Vehicle user = _vehicleService.GetVehicleById(id);
                _vehicleService.DeleteVehicle(id);
                return RedirectToAction("Index", "Vehicle");
            }
            catch (Exception ex)
            {
                //      return RedirectToAction("Delete",
                //      new System.Web.Routing.RouteValueDictionary {
                //{ "id", id },
                //{ "saveChangesError", true } });
                return View();
            }

        }
        public ActionResult Edit(int id)
        {
            Vehicle model = _vehicleService.GetVehicleById(id);
            TempData["Oldname"] = model.VehicleName;
            return View(model);
        }
        [HttpPost]
        public ActionResult Edit(Vehicle Vehicle)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var oldname = TempData["Oldname"];
                    _vehicleService.UpdateVehicle(Vehicle, oldname);
                    return RedirectToAction("Index", "Vehicle");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
            }
            return View(Vehicle);
        }
    }
}
