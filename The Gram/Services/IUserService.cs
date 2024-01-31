using Microsoft.AspNetCore.Identity;
using The_Gram.Data.Models;

namespace The_Gram.Services
{
    public interface IUserService
    {
        Task<User> GetByUsernameAsync(string username);
        Task<User> GetByEmailAsync(string email);
        Task<bool> MakeUserAsync(User user, string password);
        Task<User> GetByIdAsync(string id);
        Task<bool> SignInUserAsync(User user, string password);
        Task<string> CreateEmailConfirmationTokenAsync(User user);
        Task<IdentityResult> ConfirmEmailAsync(User user, string token);
        Task<bool> CheckPasswordAsync(User user, string password);
        Task<bool> DeleteUserAsync(string username, string password);
        Task<bool> Edit(string id, string fullName, string pictureUr, string bio, string username);
    }
}
