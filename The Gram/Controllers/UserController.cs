using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using The_Gram.Data.Models;
using The_Gram.Models.User;
using The_Gram.Services;

namespace The_Gram.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        private readonly UserManager<User> userManager;

        private readonly SignInManager<User> signInManager;
        private readonly IUserService userService;

        public UserController(
            UserManager<User> _userManager,
            SignInManager<User> _signInManager,
            IUserService _userService)
        {
            userManager = _userManager;
            signInManager = _signInManager;
            userService = _userService;
        }

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
                return RedirectToAction("Index", "Home");
            }
            return View(model);
        }
    }
}
