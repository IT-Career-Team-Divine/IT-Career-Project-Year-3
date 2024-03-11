using Microsoft.AspNetCore.Identity;
using The_Gram.Data.Models;
using The_Gram.Data;
using The_Gram.Models.Admin;
using Microsoft.EntityFrameworkCore;

namespace The_Gram.Services
{
    public class AdminService : IAdminService
    {
        private readonly ApplicationDbContext context;
        private readonly UserManager<User> userManager;
        private readonly IUserService userService;
        public AdminService(ApplicationDbContext dbContext, UserManager<User> manager, IUserService userService)
        {
            this.context = dbContext;
            this.userManager = manager;
            this.userService = userService;

        }

        public async Task<bool> IsAdminAsync(User user, UserProfile profile)
        {

            if (user == null || profile == null)
            {
                return false;
            }
            if (profile.UserId == user.Id && user.CurrentProfileId == profile.Id)
            {
                var isInRole = await userManager.IsInRoleAsync(user, "Admin");

                var profileIsAdmin = profile.IsAdmin;
                if (isInRole && profileIsAdmin)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            }
            else
            {
                return false;
            }
        }

        public async Task<bool> MakeAdminAsync(User user, UserProfile profile)
        {
            bool output = false;
            if (user == null || profile == null)
            {
                return false;
            }
            if (profile.UserId == user.Id)
            {

                var application = await context.adminApplications.FirstOrDefaultAsync(adp => adp.ApplicantId == profile.Id);
               
                var deleteAplication = context.adminApplications.Remove(application);
                if (await userManager.IsInRoleAsync(user, "Admin") == false)
                {
                    var operation = await userManager.AddToRoleAsync(user, "Admin");
                    var remove = await userManager.RemoveFromRoleAsync(user, "User");
                    if (operation.Succeeded && remove.Succeeded)
                    {
                        output = true;
                    }
                }
            }
            profile.IsAdmin = output;
            await context.SaveChangesAsync();
            return profile.IsAdmin;
        }


        public async Task<bool> MakeAdminApplicationAsync(BecomeAdminApplicationViewModel aplicationModel, User currentUser)
        {
            bool output = false;
            if (aplicationModel == null || currentUser == null)
            {
                return output;
            }
            User user = await userService.GetByEmailAsync(aplicationModel.Email);
            if (user == null)
            {
                return output;
            }
            var profile = await userService.GetByUsernameAsync(aplicationModel.Username);
            if (profile == null)
            {
                return output;
            }
            if (profile.Id != user.CurrentProfileId)
            {
                return output;
            }
            var application = new BecomeAdminApplication()
            {
                Applicant = profile,
                ApplicantId = profile.Id,
            };
            var operation = await context.adminApplications.AddAsync(application);
            if (context.SaveChangesAsync().Result > 0 && operation != null)
            {
                output = true;
            }
            return output;
        }

        public List<BecomeAdminApplication> GetAllAdminApplicationsAsync()
        {
            var applications = context.adminApplications.Select(app => new BecomeAdminApplication()
            {
                Applicant = app.Applicant,
                ApplicantId = app.Id,
                Id = app.Id
            });
            return applications.ToList();
        }
    }
}