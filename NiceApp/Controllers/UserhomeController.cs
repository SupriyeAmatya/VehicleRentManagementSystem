using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NiceApp.Data;
using NiceApp.Models.DataModel;
using NiceApp.Services.EmailServices;

namespace NiceApp.Controllers
{
    public class UserhomeController : Controller
    {
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
            RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailSenderServices = emailSenderServices;
            _roleManager = roleManager;
            _db = db;
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

    }
}
