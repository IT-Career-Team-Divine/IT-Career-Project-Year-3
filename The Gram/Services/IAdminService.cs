using The_Gram.Data.Models;
using The_Gram.Models.Admin;

namespace The_Gram.Services
{
    public interface IAdminService
    {
        Task<bool> MakeAdminAsync(User user);
        Task<bool> IsAdminAsync(User user);
        Task<bool> MakeAdminApplicationAsync(BecomeAdminApplicationViewModel application);
    }
}
