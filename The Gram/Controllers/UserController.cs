using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using The_Gram.Data.Models;
using The_Gram.Models.User;
using The_Gram.Services;
using static Org.BouncyCastle.Crypto.Engines.SM2Engine;

namespace The_Gram.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        private readonly SignInManager<User> signInManager;
        private readonly IUserService userService;
        private readonly IEmailSender emailSender;

        public UserController(
            SignInManager<User> _signInManager,
            IUserService _userService,
            IEmailSender _emailSender)
        {
            signInManager = _signInManager;
            userService = _userService;
            emailSender = _emailSender;        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register()
        {
            if (User?.Identity?.IsAuthenticated ?? false)
            {
                return RedirectToAction("Index", "Home");
            }

            var model = new RegisterViewModel();

            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = new User()
            {
                Email = model.Email,
                FullName = model.FullName,
                UserName = model.UserName
            };


            var madeUser = await userService.MakeUserAsync(user, model.Password);
            if (madeUser == true)
            {
                var token = await userService.CreateEmailConfirmationTokenAsync(user);
                var callbackUrl = Url.Action("ConfirmEmail", "User", new {user.Id,token },Request.Scheme);
                await emailSender.SendEmailAsync(user.Email, "Confirm your account",
               $"Please confirm your account by clicking this link: <a href='{callbackUrl}'>link</a>");
                return View(nameof(SuccessRegistration));
            }
     //       ModelState.AddModelError("", "Invalid register");
            return View(model);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Login()
        {
            if (User?.Identity?.IsAuthenticated ?? false)
            {
                return RedirectToAction("Index", "Home");
            }

            var model = new LoginViewModel();

            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await userService.GetByUsernameAsync(model.Username);

            if (user != null)
            {
                var userSignedIn = await userService.SignInUserAsync(user, model.Password);

                if (userSignedIn == true)
                {
                    return RedirectToAction("Index", "Home");
                }
            }

            ModelState.AddModelError("", "Invalid login");

            return View(model);
        }

        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();

            return RedirectToAction("Index", "Home");
        }
        [HttpGet]
        public async Task<IActionResult> ConfirmEmail(string token, string email)
        {
            var user = await userService.GetByEmailAsync(email);
            if (user == null)
                return View("Error");
            var result = await userService.ConfirmEmailAsync(user, token);
            if (result.Succeeded)
            {
                return View("ConfirmEmail");
            }
            else
            {
                return View("Error");
            }
        }
        [HttpGet]
        public IActionResult SuccessRegistration()
        {
            return View();
        }
    }
}
