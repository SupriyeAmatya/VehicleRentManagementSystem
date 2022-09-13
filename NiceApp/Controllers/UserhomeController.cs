using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using NiceApp.Data;
using NiceApp.Models.DataModel;
using NiceApp.Services.EmailServices;
using NiceApp.Services.UserhomeService;

namespace NiceApp.Controllers
{
    public class UserhomeController : Controller
    {
        private readonly ILogger<UserhomeController> _logger;
        private IUserhomeService _UserhomeService;
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IEmailSenderServices _emailSenderServices;
        private readonly AppDbContext _db;
        public UserhomeController(
            UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager,
            IEmailSenderServices emailSenderServices,
            AppDbContext db,
            RoleManager<IdentityRole> roleManager,
            ILogger<UserhomeController> logger, 
            IUserhomeService UserhomeService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailSenderServices = emailSenderServices;
            _roleManager = roleManager;
            _db = db;

            _logger = logger;
            _UserhomeService = UserhomeService;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            return View();
        }

        public IActionResult Contact()
        {
            return View();
        }
        [HttpGet]
        public IActionResult AllVehicles()
        {


            return View();
        }
        [HttpPost]
        public IActionResult AllVehicles(string VehicleType)
        {
            //ViewBag.AllVehicleType = VehicleType;

            var VehicleTypeList = _UserhomeService.GetAllVehicleFromType(VehicleType);
            //ViewBag.VehicleList = VehicleTypeList;

            return View(VehicleTypeList);
        }


        public IActionResult AllStations()
        {
            ViewBag.AllVehiclesListStation = new SelectList(_UserhomeService.GetAllStation(), "Id", "StationName");
            return View();
        }
        [HttpPost]
        public IActionResult AllStations(int station)
        {

            ViewBag.AllVehiclesListStation = new SelectList(_UserhomeService.GetAllStation(), "Id", "StationName");
          
            var VehicleSationList = _UserhomeService.GetAllVehicleFromStation(station);
            ViewBag.VehicleStList = VehicleSationList;
            return View();
        }

    }
}
