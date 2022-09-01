using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using NiceApp.Data;
using NiceApp.Models.DataModel;
using NiceApp.Models.ViewModel;
using NiceApp.Services.EmailServices;

namespace NiceApp.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IEmailSenderServices _emailSenderServices;
        private readonly AppDbContext _db;
        public AccountController(
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

        //Forgot Password Function
        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user == null)
                {
                    return RedirectToAction("ForgotPasswordConfirmation");
                }
                var code = await _userManager.GeneratePasswordResetTokenAsync(user);
                var mailAddress = new EmailAddress { DisplayName = model.Email, Address = model.Email };
                var callbackurl = Url.Action("ResetPassword", "Account", new { userId = user.Id, code = code }, protocol: HttpContext.Request.Scheme);
                var message = new Message(new EmailAddress[] { mailAddress }, "Email Confirmation", $"Confirm your email by <a href = \"" + callbackurl + "\"> clicking here </a>.", null);
                await _emailSenderServices.SendEmailAsync(message);
                return RedirectToAction("ForgotPasswordConfirmation");
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        //Reset Password Function
        [HttpGet]
        public IActionResult ResetPassword(string code = null)
        {
            return code == null ? View("Error") : View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel resetPasswordViewModel)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(resetPasswordViewModel.Email);
                if (user == null)
                {
                    ModelState.AddModelError("Email", "User not found.");
                    return View();
                }
                var result = await _userManager.ResetPasswordAsync(user, resetPasswordViewModel.Code, resetPasswordViewModel.Password);
                if (result.Succeeded)
                {
                    return RedirectToAction("ResetPasswordConfirmation");
                }
            }
            return View(resetPasswordViewModel);
        }

        public IActionResult ResetPasswordConfirmation()
        {
            return View();
        }

        //login function
        public IActionResult Login(string? returnUrl = null)
        {
            LoginViewModel loginViewModel = new LoginViewModel();
            loginViewModel.ReturnUrl = returnUrl ?? Url.Content("~/");
            return View(loginViewModel);
        }

        public IActionResult PasswordExpiry()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> PasswordExpiry(PasswordExpiryViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user == null)
                {
                    return RedirectToAction("PasswordExpiry");
                }
                //var verifyCodeOne = await _userManager.GeneratePasswordResetTokenAsync(user);
                //var link = Url.Action(nameof(ExtendPasswordExpiry), "Account", new { userId = user.Id, verifyCodeOne }, Request.Scheme, Request.Host.ToString());
                //var mailAddress = new EmailAddress { DisplayName = user.Email, Address = user.Email };
                //var message = new Message(new EmailAddress[] { mailAddress }, "Email Verification", $"Verify your email by <a href = \"" + link + "\"> clicking here </a>.", null);
                //await _emailSenderServices.SendEmailAsync(message);
                var code = await _userManager.GeneratePasswordResetTokenAsync(user);
                var mailAddress = new EmailAddress { DisplayName = model.Email, Address = model.Email };
                var callbackurl = Url.Action("ExtendPasswordExpiry", "Account", new { userId = user.Id, code = code }, protocol: HttpContext.Request.Scheme);
                var message = new Message(new EmailAddress[] { mailAddress }, "Email Confirmation", $"Confirm your email by <a href = \"" + callbackurl + "\"> clicking here </a>.", null);
                await _emailSenderServices.SendEmailAsync(message);
                return RedirectToAction("ForgotPasswordConfirmation");
            }
            return View(model);
        }
        [HttpGet]
        public IActionResult ExtendPasswordExpiry(string code = null)
        {
            return code == null ? View("Error") : View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ExtendPasswordExpiry(ExtendPasswordExpiryViewModel extendPasswordExpiryViewModel)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(extendPasswordExpiryViewModel.Email);
                if (user == null)
                {
                    ModelState.AddModelError("Email", "User not found.");
                    return View();
                }
                var dateToday = DateTime.Now;
                var extendDate = DateTime.Now.AddYears(2);
                if (user.PwdExpiry > dateToday)
                {
                    return View("Error");
                }
                user.PwdExpiry = extendDate;
                var resultOne = await _userManager.UpdateAsync(user);
                var result = await _userManager.ResetPasswordAsync(user, extendPasswordExpiryViewModel.Code, extendPasswordExpiryViewModel.Password);
                if (result.Succeeded && resultOne.Succeeded)
                {
                    return RedirectToAction("ResetPasswordConfirmation");
                }
            }
            return View(extendPasswordExpiryViewModel);
        }

        public IActionResult ExtendPasswordExpiryConfirmation()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel loginViewModel, string? returnUrl)
        {
            if (ModelState.IsValid)
            {

                var user = await _userManager.FindByNameAsync(loginViewModel.UserName);
                //Checking User Status
                if (user.UserStatus != 1)
                {
                    return RedirectToAction("InvalidUserStatus");
                }
                //Changing the User Status to active after the user gets logged in to the system.
                user.LoginStatus = "Active";
                var statusChange = await _userManager.UpdateAsync(user);
                //Check the password Expiry Date.
                var dateCheck = DateTime.Now;
                if (dateCheck > user.PwdExpiry)
                {
                    return RedirectToAction("PasswordExpiry");
                }

                var result = await _signInManager.PasswordSignInAsync(loginViewModel.UserName, loginViewModel.Password, loginViewModel.RememberMe, lockoutOnFailure: true);
                if (result.Succeeded && statusChange.Succeeded)
                {

                    return RedirectToAction("Index", "Userhome");
                }
                if (result.IsLockedOut)
                {

                    return View("Lockout");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    return View(loginViewModel);
                }
            }
            return View(loginViewModel);
        }

        public IActionResult InvalidUserStatus() => View();














        //Register Function
        public async Task<IActionResult> Register(string? returnUrl = null)
        {
            if (!await _roleManager.RoleExistsAsync("Admin"))
            {
                await _roleManager.CreateAsync(new IdentityRole("Admin"));
                await _roleManager.CreateAsync(new IdentityRole("User"));
            }

            List<SelectListItem> listItems = new List<SelectListItem>();
            listItems.Add(new SelectListItem()
            {
                Value = "User",
                Text = "User"
            });


            RegisterViewModel registerViewModel = new RegisterViewModel();
            //registerViewModel.RoleList = listItems;
            registerViewModel.ReturnUrl = returnUrl;
            return View(registerViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel registerViewModel, string? returnUrl = null)
        {
            registerViewModel.ReturnUrl = returnUrl;
            returnUrl = returnUrl ?? Url.Content("~/");
            var dateOne = DateTime.Now.AddMonths(3);
            if (ModelState.IsValid)
            {
                var user = new AppUser { Email = registerViewModel.Email, PwdExpiry = dateOne, LoginStatus = "InActive", UserStatus = 1, UserName = registerViewModel.UserName };
                var result = await _userManager.CreateAsync(user, registerViewModel.Password);
                if (result.Succeeded)
                {
                    //generate email verificaion token
                    var verifyCode = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    var link = Url.Action(nameof(VerifyEmail), "Account", new { userId = user.Id, verifyCode }, Request.Scheme, Request.Host.ToString());
                    var mailAddress = new EmailAddress { DisplayName = user.Email, Address = user.Email };
                    var message = new Message(new EmailAddress[] { mailAddress }, "Email Verification", $"Verify your email by <a href = \"" + link + "\"> clicking here </a>.", null);
                    await _emailSenderServices.SendEmailAsync(message);

                    //Generate roles for the users
                    await _userManager.AddToRoleAsync(user, "User");
                    //if (registerViewModel.RoleSelected != null && registerViewModel.RoleSelected.Length > 0 && registerViewModel.RoleSelected == "User")
                    //{
                    //    await _userManager.AddToRoleAsync(user, "User");
                    //}
                    //else
                    //{
                    //    await _userManager.AddToRoleAsync(user, "Admin");
                    //}

                    //await _signInManager.SignInAsync(user, isPersistent: false);

                    return RedirectToAction("EmailVerification");
                }
                ModelState.AddModelError("Email", "User could not be created. Password not unique enough");

            }

            return View(registerViewModel);

        }

        public async Task<IActionResult> VerifyEmail(string userId, string verifyCode)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return View("Error");
            }
            var result = await _userManager.ConfirmEmailAsync(user, verifyCode);
            if (result.Succeeded)
            {
                return View();
            }

            return View("Error");
        }
        public IActionResult EmailVerification() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LogOff()
        {
            var user = await _userManager.GetUserAsync(User);
            user.LoginStatus = "InActive";
            var statusChange = await _userManager.UpdateAsync(user);
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Userhome");
        }
    }
}
