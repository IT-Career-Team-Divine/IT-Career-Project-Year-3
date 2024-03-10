using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using The_Gram.Data;
using The_Gram.Data.Models;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using The_Gram.Models.User;

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

        public async Task<User> GetByIdAsync(string id)
        {
            var user = await context.Users.FirstOrDefaultAsync(u => u.Id == id);
            return (User)user;
        }

        public async Task<UserProfile> GetByUsernameAsync(string username)
        {
            UserProfile user = await context.UserProfiles.FirstOrDefaultAsync(u => u.Username == username);
            return user;
        }

        public async Task<bool> MakeUserAsync(User user, RegisterViewModel model)
        {
            var output = false;
            var userExists = await this.GetByEmailAsync(model.Email);
            var valid = false;
            if (user != null)
            {
                if (userExists == null)
                {
                    var createProfile = await userManager.CreateAsync(user, model.Password);
                    if (createProfile != null)
                    {
                        var assignUserRole = await userManager.AddToRoleAsync(user, "User");
                        var assignUserRoleResult = assignUserRole;
                        if (assignUserRoleResult == null)
                        {
                            return false;
                        }

                    }
                    if (await CheckPasswordAsync(user, model.Password))
                    {
                        valid = true;
                    }
                }
                else if (await CheckPasswordAsync(userExists, model.Password))
                {
                    valid = true;
                }
                if (valid)
                {
                    output = true;
                    UserProfile profile = new UserProfile()
                    {
                        Bio = model.Bio,
                        User = user,
                        FullName = model.FullName,
                        Username = model.UserName,
                        UserId = user.Id,
                        Picture = model.PhotoUrl,
                        IsPrivate = model.IsPrivate,
                    };
                    await context.UserProfiles.AddAsync(profile);
                    await context.SaveChangesAsync();

                }




            }

            return output;
        }

        public async Task<bool> SignInUserAsync(User user, LoginViewModel model)
        {
            var profile = await GetProfileByUserIdAndUsernameAsync(user.Id, model.Username);
            if (profile == null)
            {
                return false;

            }
            user.CurrentProfileId = profile.Id;
            user.CurrentProfile = profile;
            context.SaveChangesAsync();
            await signInManager.PasswordSignInAsync(user, model.Password, false, false);
            return true;
        }

        public async Task<UserProfile> GetProfileByUserIdAndUsernameAsync(string id, string username)
        {
            var profile = await context.UserProfiles.FirstOrDefaultAsync(u => u.UserId == id && u.Username == username);
            return profile;
        }

        private async Task<bool> CheckUsernameAndPasswordAsync(string username, string password)
        {
            throw new NotImplementedException();
        }

        public async Task<User> GetByEmailAsync(string email)
        {
            User user = (User)await context.Users.FirstOrDefaultAsync(u => u.Email == email);
            return user;
        }
        public async Task<UserProfile> GetProfileByUserIdAsync(string id)
        {
            var profile = await context.UserProfiles.FirstOrDefaultAsync(p => p.User.Id == id);
            return profile;
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
            var profile = await GetByUsernameAsync(username);

            if (profile == null)
            {
                return false;
            }
            var user = await GetByIdAsync(profile.UserId);

            var isValidPassword = await CheckPasswordAsync(user, password);

            if (!isValidPassword)
            {
                return false;
            }

            var result = context.UserProfiles.Remove(profile);
            if (result != null)
            {
                user.CurrentProfileId = null;
                context.SaveChanges();
                return true;

            }
            return false;
        }

        public async Task<bool> CheckPasswordAsync(User user, string password)
        {
            var result = await signInManager.CheckPasswordSignInAsync(user, password, false);
            return result.Succeeded;
        }

        public async Task<bool> Edit(string id, string fullName, string pictureUr, string bio, string username)
        {
            var userData = await GetProfileByIdAsync(id);

            if (userData == null)
            {
                return false;
            }
            userData.Id= id;
            userData.FullName = fullName;
            userData.Picture = pictureUr;
            userData.Bio = bio;
            userData.Username = username;

            this.context.SaveChanges();

            return true;
        }

        public async Task<UserProfile> GetProfileByIdAsync(string id)
        {
            var profile = await context.UserProfiles.FirstOrDefaultAsync(u => u.Id == id);
            return profile;
        }

    }
}
