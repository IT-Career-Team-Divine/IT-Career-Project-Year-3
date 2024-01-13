using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using The_Gram.Data;
using The_Gram.Data.Models;

namespace The_Gram.Services
{
    public class UserService : IUserService
    {

        private readonly ApplicationDbContext context;
        private readonly UserManager<User> userManager;

        public UserService(ApplicationDbContext context, UserManager<User> userManager)
        {
            this.context = context;
            this.userManager = userManager;
        }

        public Task<User> GetByIdAsync(string id)
        {
            var user = userManager.FindByIdAsync(id);
            return user;
        }

        public async Task<User> GetByUsernameAsync(string username)
        {
            User user = await userManager.FindByNameAsync(username);
            return user;
        }

        public Task<bool> IsAdminAsync(User user)
        {

            return userManager.IsInRoleAsync(user, "Admin");
        }

        //public async Task<bool> MakeAdminAsync(User user)
        //{
        //    bool output = false;
        //    int count = GetCount();
        //    if (count == 1)
        //    {
        //        var operation = await userManager.AddToRoleAsync(user, "Admin");
        //        var remove = await userManager.RemoveFromRoleAsync(user, "User");
        //
        //        if (operation.Succeeded && remove.Succeeded)
        //        {
        //            output = true;
        //        }
        //    }
        //
        //    return output;
        //}


        public async Task<bool> MakeUserAsync(User user, string password)
        {
            bool output = false;
            var createUser = await userManager.CreateAsync(user, password);
            var assignUserRole = await userManager.AddClaimAsync(user, new Claim(ClaimTypes.Role, "user"));
            var assignUserRoleResult = assignUserRole;

            if (createUser.Succeeded && assignUserRoleResult.Succeeded)
            {
                output = true;
            }
            return output;
        }

        public async Task<User> GetByUserNameAsync(string? name)
        {
            User user = await userManager.FindByNameAsync(name);
            return user;
        }

        public Task<bool> MakeAdminAsync(User user)
        {
            throw new NotImplementedException();
        }
    }
}
