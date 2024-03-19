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

        public async Task<bool> FriendRequestSent(string id, string friendId)
        {
            var output = false;
            var isRequestSentFromUserToFriend = await context.ProfileFriendMappings.Where(fr => fr.UserId== id && fr.Friend.Id == friendId).AnyAsync();
          
             if (isRequestSentFromUserToFriend == true)
            {
                output = isRequestSentFromUserToFriend;
            }
            return output;
        }

        public async Task<bool> IsFriend(string friendId, string id)
        {
            var isRequestSentFromUserToFriend = await context.ProfileFriendMappings.Where(fr => fr.UserId == id && fr.Friend.Id == friendId && fr.isAccepted == true).AnyAsync();
            var isRequestSentFromFriendToUser = await context.ProfileFriendMappings.Where(fr => fr.UserId == friendId && fr.Friend.Id == id && fr.isAccepted == true).AnyAsync();
            if (isRequestSentFromFriendToUser || isRequestSentFromUserToFriend)
            {
                return true;
            }
            return false;
        }

        public async Task<List<AllUsersViewModel>> Search(string query)
        {
            var result = context.UserProfiles.Where(u => u.Username.Contains(query) == true).ToList();
            var users = new List<AllUsersViewModel>();
            foreach (var user in result)
            {
                var allProfileViewModel = new AllUsersViewModel()
                {
                    Username= user.Username,
                    Id= user.Id,
                    Picture = user.Picture,
                };
                users.Add(allProfileViewModel);
            }
            return users;
        }

        public async Task<bool> SendFriendRequest(string id, string friendId)
        {
            var userProfiile = await GetProfileByIdAsync(id);
            var friendProfile = await GetProfileByIdAsync(friendId);
            if (userProfiile == null || friendProfile == null) 
            {
                return false;
            }
            ProfileFriendMapping mapping = new ProfileFriendMapping()
            {
                UserId = id,
                FriendId = friendId
            };
           await context.ProfileFriendMappings.AddAsync(mapping);
           await context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> CancelFriendRequest(string id, string modelId)
        {
            var mapping = await context.ProfileFriendMappings.FirstOrDefaultAsync(pfr => pfr.FriendId == modelId  && pfr.UserId == id);
            if (mapping == null)
            {
                return false;
            }
            context.ProfileFriendMappings.Remove(mapping);
           await context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> AcceptFreindRequest(string modelId, string id)
        {
            var mapping = await context.ProfileFriendMappings.FirstOrDefaultAsync(pfr => pfr.UserId == modelId && pfr.FriendId == id);
            if (mapping == null)
            {
                return false;
            }
           await Unfollow(modelId, id);
            mapping.isAccepted = true;
            await context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> Follows(string friendId, string id)
        {
           var follows = await context.ProfileFollowerMappings.Where(pfm => pfm.Follower.Id == friendId && pfm.FollowingId == id).AnyAsync();
            return follows;
        }

        public async Task<bool> Follow(string modelId, string id)
        {
            var follower = await GetProfileByIdAsync(id);
            var profile = await GetProfileByIdAsync(modelId);
            ProfileFollowerMapping profileFollowerMapping = new ProfileFollowerMapping()
            {
                FollowerId = id,
                FollowingId = modelId,
            };
          await  context.ProfileFollowerMappings.AddAsync(profileFollowerMapping);
            await context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> Unfollow(string modelId, string id)
        {
            var follower = await GetProfileByIdAsync(id);
            var profile = await GetProfileByIdAsync(modelId);
            if (follower == null || profile == null)
            {
                return false;
            }
            var mapping = await context.ProfileFollowerMappings.FirstOrDefaultAsync(pfm => pfm.FollowingId == modelId && pfm.FollowerId == id);
            if (mapping == null)
            {
                return false;
            }
            context.ProfileFollowerMappings.Remove(mapping);
           await context.SaveChangesAsync();
            return true;
        }
        public async Task<bool> Defriend(string modelId, string id)
        {
            var friend = await GetProfileByIdAsync(modelId);
            var profile = await GetProfileByIdAsync(id);
            if (friend == null || profile == null)
            {
                return false;
            }
            var mapping = await context.ProfileFriendMappings.FirstOrDefaultAsync(pfm => pfm.UserId == modelId && pfm.FriendId == id);
            if (mapping == null)
            {
                return false;
            }
            context.ProfileFriendMappings.Remove(mapping);
            await context.SaveChangesAsync();
            return true;
        }
    }
}
