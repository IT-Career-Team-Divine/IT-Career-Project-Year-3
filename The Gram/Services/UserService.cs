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

        public async Task<bool> MakeUserAsync(User user, string password)
        {
            bool output = false;
            var createUser = await userManager.CreateAsync(user, password);
            if (createUser.Succeeded)
            {
                var assignUserRole = await userManager.AddToRoleAsync(user, "User");
                var assignUserRoleResult = assignUserRole;
                if (createUser.Succeeded && assignUserRoleResult.Succeeded)
                {
                    output = true;
                }
            }
            else
            {
                output = false;
            }
            return output;
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
        public async Task<bool> DeleteUserAsync(string username, string password)
        {
            var user = await GetByUsernameAsync(username);

            if (user == null)
            {
                return false;
            }

            var isValidPassword = await CheckPasswordAsync(user, password);

            if (!isValidPassword)
            {
                return false;
            }

            var result = await userManager.DeleteAsync(user);

            return result.Succeeded;
        }

        public async Task<bool> CheckPasswordAsync(User user, string password)
        {
            var result = await userManager.CheckPasswordAsync(user, password);
            return result;
        }

        public async Task<bool> Edit(string id, string fullName, string pictureUr, string bio, string username)
        {
            var userData = await GetByIdAsync(id);

            if (userData == null)
            {
                return false;
            }

            userData.FullName = fullName;
            userData.Picture = pictureUr;
            userData.Bio = bio;
            userData.UserName = username;

            this.context.SaveChanges();

            return true;
        }
    }
}
