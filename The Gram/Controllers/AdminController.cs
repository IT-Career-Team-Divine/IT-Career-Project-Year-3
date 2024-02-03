using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using The_Gram.Data.Models;
using The_Gram.Models.Admin;
using The_Gram.Services;

namespace The_Gram.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        public IAdminService adminService;
        public UserManager<User> userManager;
        public AdminController(IAdminService adminService, UserManager<User> userManager)
        {
            this.adminService = adminService;
            this.userManager = userManager;
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
            var result = await adminService.MakeAdminApplicationAsync(model);
            if (result == null)
            {
                return View(model);
            }
            return View(nameof(SuccessApplication));
        }
        public IActionResult SuccessApplication()
        {
            return View();
        }
    }

    
}
