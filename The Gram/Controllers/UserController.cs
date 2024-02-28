using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Security.Certificates;
using The_Gram.Data.Models;
using The_Gram.Models.User;
using The_Gram.Services;
using static The_Gram.Data.Constants.Constants.UserConstants;
using static Org.BouncyCastle.Crypto.Engines.SM2Engine;
using Microsoft.EntityFrameworkCore;

namespace The_Gram.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        private readonly SignInManager<User> signInManager;
        private readonly IUserService userService;
        private readonly IEmailSender emailSender;
        private readonly UserManager<User> userManager;

        public UserController(
            SignInManager<User> _signInManager,
            IUserService _userService,
            IEmailSender _emailSender, UserManager<User> _userManager)
        {
            signInManager = _signInManager;
            userService = _userService;
            emailSender = _emailSender;
            userManager = _userManager;
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
            User userWithEmailExists = await userService.GetByEmailAsync(model.Email);
            User UserWithUsernameExists = await userService.GetByUsernameAsync(model.UserName);
            if (userWithEmailExists != null)
            {
                ModelState.AddModelError("", "Email is already registered");
            }
            if (UserWithUsernameExists != null)
            {
                ModelState.AddModelError("", "Username is taken, please choose another");
            }
            if (ModelState.ErrorCount != 0)
            {
                return View(model);
            }
            if (model.Bio == null)
            {
                model.Bio = defaultBio;
            }
            var user = new User()
            {
                Email = model.Email,
                FullName = model.FullName,
                UserName = model.UserName,
                Bio = model.Bio
            };


            var madeUser = await userService.MakeUserAsync(user, model.Password);
            if (madeUser == true)
            {
                var token = await userService.CreateEmailConfirmationTokenAsync(user);
                var callbackUrl = Url.Action(nameof(ConfirmEmail), "User", new { user.Id, token, user.Email }, Request.Scheme);
                await emailSender.SendEmailAsync(user.Email, "Confirm your account",
               $"Please confirm your account by clicking this link: {callbackUrl}'");
                return View(nameof(SuccessRegistration));
            }
            return View(model);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Login(string returnUrl)
        {
            if (User?.Identity?.IsAuthenticated ?? false)
            {
                return RedirectToAction("Index", "Home");
            }
            if (Url.IsLocalUrl(returnUrl) && !string.IsNullOrEmpty(returnUrl))
            {
                ViewBag.ReturnURL = returnUrl;
            }
            var model = new LoginViewModel();

            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginViewModel model, string? returnUrl)
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
                    if (Url.IsLocalUrl(returnUrl))
                    {
                        ViewBag.ReturnUrl = returnUrl;
                        return Redirect(returnUrl);
                    }

                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", "Invalid login");
                }
            }
            else
            {
                ModelState.AddModelError("", "No such user exists");

            }


            return View(model);
        }

        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();

            return RedirectToAction("Index", "Home");
        }
        [HttpGet]
        [Authorize]
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
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Delete(string id)
        {
            var userToDelete = await this.userService.GetByIdAsync(id);
            var currentUser = await userManager.GetUserAsync(HttpContext.User);
            var userIsAdmin = await userManager.IsInRoleAsync(currentUser, "Admin");
            if (id == null)
            {
                ModelState.AddModelError("", "There isn't such a user");
                return RedirectToAction("Index", "Home");
            }
            if (userToDelete != currentUser && !userIsAdmin)
            {
                ModelState.AddModelError("", "This isn't your account and you are not an Administrator, you have no premission to delete it");
                return RedirectToAction("Index", "Home");
            }
            DeletionViewModel model = new DeletionViewModel();
            return View(model);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Delete(string id,DeletionViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await userService.GetByUsernameAsync(model.Username);

            if (user == null)
            {
                ModelState.AddModelError("", "Invalid username");
                return View(model);
            }
           

            var passwordCorrect = await userService.CheckPasswordAsync(user, model.Password);

            if (!passwordCorrect)
            {
                ModelState.AddModelError("", "Invalid password");
                return View(model);
            }

            var deleted = await userService.DeleteUserAsync(model.Username, model.Password);

            if (deleted)
            {
                await signInManager.SignOutAsync();
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ModelState.AddModelError("", "Failed to delete user");
                return View(model);
            }
        }
        [HttpGet]
        public async Task<IActionResult> Account(string id)
        {
            var user = await this.userService.GetByIdAsync(id);
            var userAccountViewModel = new UserAccountViewModel(){
            Id = id,
            PictureUr = user.Picture,
            FullName= user.FullName,
            Bio = user.Bio,
            Username = user.UserName
            };
            return View(userAccountViewModel);
        }
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Edit(string id)
        {
            var user = await this.userService.GetByIdAsync(id);
            var currentUser = await userManager.GetUserAsync(HttpContext.User);
            var userIsAdmin = await userManager.IsInRoleAsync(currentUser, "Admin");
            var userAccountViewModel = new UserAccountViewModel()
            {
                Id = id,
                PictureUr = user.Picture,
                FullName = user.FullName,
                Bio = user.Bio,
                Username = user.UserName
            };

            if (currentUser.Id != id && !userIsAdmin)
            {
                return RedirectToAction("Become", "Admin");
            }

            return View(userAccountViewModel);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Edit(string id, UserAccountViewModel model)
        {

            if (!ModelState.IsValid)
            {
                return View(model);
            }


            var edited = await this.userService.Edit(
                id,
                model.FullName,
                model.PictureUr,
                model.Bio,
                model.Username);
            if (!edited)
            {
                return BadRequest();
            }
            return Redirect($"~/User/Account/{id}");
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> UserSearch(SearchUsersViewModel model, string? returnUrl)
        {
            
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            if (string.IsNullOrEmpty(model.Query))
            {
                ModelState.AddModelError("", "Please enter a search query.");
                return View(model);
            }

            var user = await userService.GetByUsernameAsync(model.Query); 

            if (user == null)
            {
                ModelState.AddModelError("", "No user found with the specified username.");
                return View(model);
            }

            model.SearchResults = new List<User> { user };

            return View(model);
        }

    }
}
