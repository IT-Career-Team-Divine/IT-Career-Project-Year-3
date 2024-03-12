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
using System;
using The_Gram.Models;

namespace The_Gram.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        private readonly SignInManager<User> signInManager;
        private readonly IUserService userService;
        private readonly IEmailSender emailSender;
        private readonly UserManager<User> userManager;
        private readonly IAdminService adminService;

        public UserController(
            SignInManager<User> _signInManager,
            IUserService _userService,
            IEmailSender _emailSender, UserManager<User> _userManager, IAdminService _adminService)
        {
            signInManager = _signInManager;
            userService = _userService;
            emailSender = _emailSender;
            userManager = _userManager;
            adminService = _adminService;
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
            var user = new User();
            var userExists = await userService.GetByEmailAsync(model.Email);
            var madeUser = false;

            if (!ModelState.IsValid)
            {
                return View(model);
            }
            UserProfile UserWithUsernameExists = await userService.GetByUsernameAsync(model.UserName);
            if (UserWithUsernameExists != null)
            {
                ModelState.AddModelError("", "Username is taken, please choose another");
            }
            if (!model.AcceptTermsAndConditions)
            {
                ModelState.AddModelError("AcceptTermsAndConditions", "You must accept the terms and conditions.");
            }
            if (ModelState.ErrorCount != 0)
            {
                return View(model);
            }
            if (model.Bio == null)
            {
                model.Bio = defaultBio;
            }
            if (model.PhotoUrl == null)
            {
                model.PhotoUrl = defaultPicture;
            }
            if (userExists == null)
            {
                user.Email = model.Email;
                user.UserName = model.Email;
                madeUser = await userService.MakeUserAsync(user, model);
            }
            else
            {
                madeUser = await userService.MakeUserAsync(userExists, model);

            }
            if (madeUser == true)
            {
                if (userExists != null)
                {
                    return View(nameof(SuccessRegistrationOfAnotherProfile));
                }
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
            var user = await userService.GetByEmailAsync(model.Email);


            if (user != null)
            {
                var userSignedIn = await userService.SignInUserAsync(user, model);
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
            if (id == null)
            {
                ModelState.AddModelError("", "There isn't such a user");
                return RedirectToAction("Index", "Home");
            }

            var currentUser = await userManager.GetUserAsync(HttpContext.User);
            var userToDelete = await userService.GetProfileByIdAsync(currentUser.CurrentProfileId);
            var userIsAdmin = await userManager.IsInRoleAsync(currentUser, "Admin");

            if (userToDelete.User.Id != currentUser.Id && !userIsAdmin)
            {
                ModelState.AddModelError("", "This isn't your account and you are not an Administrator, you have no premission to delete it");
                return RedirectToAction("Index", "Home");
            }
            DeletionViewModel model = new DeletionViewModel();
            return View(model);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Delete(string id, DeletionViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var profile = await userService.GetByUsernameAsync(model.Username);

            if (profile == null)
            {
                ModelState.AddModelError("", "Invalid username");
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
            var profile = await this.userService.GetProfileByIdAsync(id);
            var userAccountViewModel = new UserAccountViewModel()
            {
                Id = id,
                PictureUr = profile.Picture,
                FullName = profile.FullName,
                Bio = profile.Bio,
                Username = profile.Username,
                IsPrivate = profile.IsPrivate,
            };
            return View(userAccountViewModel);
        }
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Edit(string id)
        {
            var profile = await this.userService.GetProfileByIdAsync(id);
            var user = await userService.GetByIdAsync(profile.UserId);
            var currentUser = await userManager.GetUserAsync(HttpContext.User);
            var userIsAdmin = await adminService.IsAdminAsync(user, profile);

            if (currentUser.Id != profile.UserId && !userIsAdmin)
            {
                return RedirectToAction("Become", "Admin");
            }
            var userAccountViewModel = new UserAccountViewModel()
            {
                Id = id,
                PictureUr = profile.Picture,
                FullName = profile.FullName,
                Bio = profile.Bio,
                Username = profile.Username
            };
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
        private static string RandomString(int length)
        {
            Random rand = new Random();
            const string pool = "abcdefghijklmnopqrstuvwxyz0123456789";
            var chars = Enumerable.Range(0, length)
                .Select(x => pool[rand.Next(0, pool.Length)]);
            return new string(chars.ToArray());
        }
        private object SuccessRegistrationOfAnotherProfile()
        {
            return View();
        }
        [Authorize]
        public async Task<IActionResult> UserSearch(SearchViewModel<AllUsersViewModel> model, string? returnUrl)
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

            var users = await userService.Search(model.Query);

            if (users == null)
            {
                ModelState.AddModelError("", "No user found with the specified username.");
                return View(model);
            }

            foreach (var user in users)
            {
                AllUsersViewModel userViewModel = new AllUsersViewModel()
                {
                    Id = user.Id,
                    Username = user.Username,
                    Picture = user.Picture
                };
                model.SearchResults.Add(userViewModel);

            }


            return View(model.SearchResults);
        }
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> SendFriendRequest(string id, string modelId)
        {
            var profile = await this.userService.GetProfileByIdAsync(id);
            var friend = await userService.GetProfileByIdAsync(modelId);
            if (profile == null || friend == null)
            {
                ModelState.AddModelError("", "Please make sure the user you are befriending exists.");

            }
            bool output = await userService.SendFriendRequest(id,modelId);
            var userAccountViewModel = new UserAccountViewModel()
            {
                Id = modelId,
                PictureUr = friend.Picture,
                FullName = friend.FullName,
                Bio = friend.Bio,
                Username = friend.Username,
                IsPrivate= friend.IsPrivate,
                
            };
            return Redirect($"~/User/Account/{modelId}");
        }
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> CancelFriendRequest(string id, string modelId)
        {
            var profile = await this.userService.GetProfileByIdAsync(id);
            var friend = await userService.GetProfileByIdAsync(modelId);
            if (profile == null || friend == null)
            {
                ModelState.AddModelError("", "Please make sure the user you are befriending exists.");

            }
            bool output = await userService.FriendRequestSent(id, modelId);
            var userAccountViewModel = new UserAccountViewModel()
            {
                Id = modelId,
                PictureUr = friend.Picture,
                FullName = friend.FullName,
                Bio = friend.Bio,
                Username = friend.Username,
                IsPrivate = friend.IsPrivate,

            };
            if (output)
            {
               await userService.CancelFriendRequest(id, modelId);
            }

            return Redirect($"~/User/Account/{modelId}");
        }
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> DeclineFriendRequest(string id, string modelId)
        {
            var profile = await this.userService.GetProfileByIdAsync(id);
            var friend = await userService.GetProfileByIdAsync(modelId);
            if (profile == null || friend == null)
            {
                ModelState.AddModelError("", "Please make sure the user you are befriending exists.");

            }
            bool output = await userService.FriendRequestSent(modelId, id);
            var userAccountViewModel = new UserAccountViewModel()
            {
                Id = modelId,
                PictureUr = friend.Picture,
                FullName = friend.FullName,
                Bio = friend.Bio,
                Username = friend.Username,
                IsPrivate = friend.IsPrivate,

            };
            if (output)
            {
                await userService.CancelFriendRequest(modelId, id);
            }

            return Redirect($"~/User/Account/{modelId}");
        }
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> AcceptFriendRequest(string id, string modelId)
        {
            var profile = await this.userService.GetProfileByIdAsync(id);
            var friend = await userService.GetProfileByIdAsync(modelId);
            if (profile == null || friend == null)
            {
                ModelState.AddModelError("", "Please make sure the user you are befriending exists.");

            }
            bool output = await userService.FriendRequestSent(modelId, id);
            var userAccountViewModel = new UserAccountViewModel()
            {
                Id = modelId,
                PictureUr = friend.Picture,
                FullName = friend.FullName,
                Bio = friend.Bio,
                Username = friend.Username,
                IsPrivate = friend.IsPrivate,

            };
            if (output)
            {
                await userService.AcceptFreindRequest(modelId, id);
            }

            return Redirect($"~/User/Account/{modelId}");
        }
        public async Task<IActionResult> Follow(string id, string modelId)
        {
            var profile = await this.userService.GetProfileByIdAsync(id);
            var friend = await userService.GetProfileByIdAsync(modelId);
            if (profile == null || friend == null)
            {
                ModelState.AddModelError("", "Please make sure the user you are befriending exists.");

            }
            bool output = await userService.Follow(modelId, id);
            var userAccountViewModel = new UserAccountViewModel()
            {
                Id = modelId,
                PictureUr = friend.Picture,
                FullName = friend.FullName,
                Bio = friend.Bio,
                Username = friend.Username,
                IsPrivate = friend.IsPrivate,

            };

            return Redirect($"~/User/Account/{modelId}");
        }
        public async Task<IActionResult> Unfollow(string id, string modelId)
        {
            var profile = await this.userService.GetProfileByIdAsync(id);
            var friend = await userService.GetProfileByIdAsync(modelId);
            if (profile == null || friend == null)
            {
                ModelState.AddModelError("", "Please make sure the user you are befriending exists.");

            }
            bool output = await userService.Unfollow(modelId, id);
            var userAccountViewModel = new UserAccountViewModel()
            {
                Id = modelId,
                PictureUr = friend.Picture,
                FullName = friend.FullName,
                Bio = friend.Bio,
                Username = friend.Username,
                IsPrivate = friend.IsPrivate,

            };

            return Redirect($"~/User/Account/{modelId}");
        }
        public async Task<IActionResult> Defriend(string id, string modelId)
        {
            var profile = await this.userService.GetProfileByIdAsync(id);
            var friend = await userService.GetProfileByIdAsync(modelId);
            if (profile == null || friend == null)
            {
                ModelState.AddModelError("", "Please make sure the user you are befriending exists.");

            }
            bool output = await userService.Defriend(modelId, id);
            var userAccountViewModel = new UserAccountViewModel()
            {
                Id = modelId,
                PictureUr = friend.Picture,
                FullName = friend.FullName,
                Bio = friend.Bio,
                Username = friend.Username,
                IsPrivate = friend.IsPrivate,

            };

            return Redirect($"~/User/Account/{modelId}");
        }

    }


}
