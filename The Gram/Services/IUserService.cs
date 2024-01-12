using The_Gram.Data.Models;

namespace The_Gram.Services
{
    public interface IUserService
    {
        Task<bool> MakeAdminAsync(User user);
        Task<User> GetByUsernameAsync(string username);
        Task<bool> IsAdminAsync(User user);
        Task<bool> MakeUserAsync(User user, string password);
        Task<User> GetByUserNameAsync(string? name);
        Task<User> GetByIdAsync(string id);
    }
}
