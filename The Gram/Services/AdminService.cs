using Microsoft.AspNetCore.Identity;
using The_Gram.Data.Models;
using The_Gram.Data;
using The_Gram.Models.Admin;

namespace The_Gram.Services
{
    public class AdminService : IAdminService
    {
        private readonly ApplicationDbContext context;
        private readonly UserManager<User> userManager;
        public AdminService(ApplicationDbContext dbContext, UserManager<User> manager) { 
          this.context = dbContext;
          this.userManager = manager;
        
        }

         public Task<bool> IsAdminAsync(User user)
         {
       
             return userManager.IsInRoleAsync(user, "Admin");
         }

       public async Task<bool> MakeAdminAsync(User user)
       {
           bool output = false;
          var operation = await userManager.AddToRoleAsync(user, "Admin");
          var remove = await userManager.RemoveFromRoleAsync(user, "User");
       
               if (operation.Succeeded && remove.Succeeded)
               {
                   output = true;
               }
       
           return output;
       }


        public async Task<bool> MakeAdminApplicationAsync(BecomeAdminApplicationViewModel aplicationModel)
        {
            bool output = false;
            User user = await userManager.FindByNameAsync(aplicationModel.Username);
            var application = new BecomeAdminApplication()
            {
                Applicant = user,
                ApplicantId = user.Id,
            };
          var operation = await context.adminApplications.AddAsync(application);
            if (context.SaveChangesAsync().Result > 0)
            {
                output = true;
            }
            return output;
        }

    }
}