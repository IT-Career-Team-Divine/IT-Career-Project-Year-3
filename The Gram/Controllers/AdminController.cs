using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using NuGet.Common;
using The_Gram.Data.Models;
using The_Gram.Models.Admin;
using The_Gram.Models.User;
using The_Gram.Services;

namespace The_Gram.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        private readonly IAdminService adminService;
        private readonly UserManager<User> userManager;
        private readonly IUserService userService;
        private readonly IEmailSender emailSender;
        public AdminController(IAdminService adminService, UserManager<User> userManager, IUserService userService, IEmailSender emailSender)
        {
            this.adminService = adminService;
            this.userManager = userManager;
            this.userService = userService;
            this.emailSender = emailSender;
        }
        [HttpGet]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> Become()
        {
            BecomeAdminApplicationViewModel model1 = new BecomeAdminApplicationViewModel();
            return View(model1);
        }
        [HttpPost]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> Become(BecomeAdminApplicationViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);

            }
            var currentUser = await userManager.GetUserAsync( HttpContext.User);
            var result = await adminService.MakeAdminApplicationAsync(model,currentUser);
            if (result == null)
            {
                return View(model);
            }
            return RedirectToAction(nameof(SuccessApplication));
        }
        public IActionResult SuccessApplication()
        {
            return View();
        }
        [HttpGet]
        [Authorize(Roles ="Admin")]
        public async  Task<IActionResult> Approve()
        {
            var user = await userManager.GetUserAsync(HttpContext.User);
            var profile = await userService.GetProfileByIdAsync(user.CurrentProfileId);
            if (profile == null || profile.IsAdmin == false)
            {
                return RedirectToAction("Index", "Home");
            }
            List<BecomeAdminApplication> applications = adminService.GetAllAdminApplicationsAsync();
            List<AllUsersViewModel> applicantViewModels = new List<AllUsersViewModel>();
            foreach (var application in applications)
            {
                var applicant = await userService.GetProfileByIdAsync(application.Applicant.Id);
                var applicantViewModel = new AllUsersViewModel
                {
                    Id = applicant.Id,
                    Picture = applicant.Picture,
                    Username = applicant.Username,
                };
                applicantViewModels.Add(applicantViewModel);
            }
            return View(applicantViewModels);
        }
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> MakeAdmin(string id)
        {
            var currentUser = await userManager.GetUserAsync(HttpContext.User);
            var currentProfile = await userService.GetProfileByIdAsync(currentUser.CurrentProfileId);
            if (currentProfile == null || currentProfile.IsAdmin == false) 
            {
                return RedirectToAction("Index", "Home");
            }
            var profileToMakeAdmin = await userService.GetProfileByIdAsync(id);
            var userToMakeAdmin = await userService.GetByIdAsync(profileToMakeAdmin.UserId);

          var result =  await adminService.MakeAdminAsync(userToMakeAdmin, profileToMakeAdmin);
            if (!result)
            {
                return RedirectToAction("Index", "Home");
            }
            var callbackUrl = Url.Action("Account", "User", new { profileToMakeAdmin.Id}, Request.Scheme);
            await emailSender.SendEmailAsync(userToMakeAdmin.Email, "Confirm your account",
           $"Your admin application has been approved the following link redirects to the profile that is now an admin: {callbackUrl}'");
            return RedirectToAction(nameof(SuccessApproval),"Admin");

        }

        public async Task<IActionResult> SuccessApproval()
        {
            return View();
        }
    }

    
}
