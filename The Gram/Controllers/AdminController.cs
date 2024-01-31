using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using The_Gram.Models.Admin;
using The_Gram.Services;

namespace The_Gram.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
       public IAdminService adminService;
        public AdminController(IAdminService adminService)
        {
            this.adminService = adminService;
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
