using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using The_Gram.Data.Models;
using The_Gram.Models.User;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace The_Gram.Services
{
    public interface IUserService
    {
        Task<UserProfile> GetByUsernameAsync(string username);
        Task<User> GetByEmailAsync(string email);
        Task<bool> MakeUserAsync(User user, RegisterViewModel model);
        Task<User> GetByIdAsync(string id);
        Task<UserProfile> GetProfileByIdAsync(string id);
        Task<bool> SignInUserAsync(User user, LoginViewModel password);
        Task<string> CreateEmailConfirmationTokenAsync(User user);
        Task<IdentityResult> ConfirmEmailAsync(User user, string token);
        Task<bool> CheckPasswordAsync(User user, string password);
        public Task<UserProfile> GetProfileByUserIdAndUsernameAsync(string id, string username);
        Task<bool> DeleteUserAsync(string username, string password);
        Task<bool> Edit(string id, string fullName, string pictureUr, string bio, string username);
        Task<bool> FriendRequestSent(string id, string friendId);
        Task<bool> IsFriend(string friendId, string id);
        Task<List<AllUsersViewModel>> Search(string query);
        Task<bool> SendFriendRequest(string id, string friendId);
        Task<bool> CancelFriendRequest(string id, string modelId);
        Task<bool> AcceptFreindRequest(string modelId, string id);
        Task<bool> Follows(string friendId, string id);
        Task<bool> Follow(string modelId, string id);
        Task<bool> Unfollow(string modelId, string id);
    }
}
