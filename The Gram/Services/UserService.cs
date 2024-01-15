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
        private readonly SignInManager<User> signInManager;

        public UserService(ApplicationDbContext context, UserManager<User> userManager, SignInManager<User> signInManager)
        {
            this.context = context;
            this.userManager = userManager;
            this.signInManager = signInManager;
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

        public async Task<bool> MakeAdminAsync(User user)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> SignInUserAsync(User user, string password)
        {
            var result = await signInManager.PasswordSignInAsync(user, password, false, false);
            bool output = false;

            if (result.Succeeded)
            {
                output = true;
            }
            return output;
        }

        public async Task<User> GetByEmailAsync(string email)
        {
            User user = await userManager.FindByEmailAsync(email);
            return user;
        }

        public async Task<string> CreateEmailConfirmationTokenAsync(User user)
        {
           string token = await userManager.GenerateEmailConfirmationTokenAsync(user);
            return token;
        }

        public async Task<IdentityResult> ConfirmEmailAsync(User user, string token)
        {
            var result = await userManager.ConfirmEmailAsync(user, token);
            return result;
        }
    }
}
