using The_Gram.Data.Models;
using The_Gram.Models.Admin;

namespace The_Gram.Services
{
    public interface IAdminService
    {
        Task<bool> MakeAdminAsync(User user, UserProfile profile);
        Task<bool> IsAdminAsync(User user, UserProfile profile);
        Task<bool> MakeAdminApplicationAsync(BecomeAdminApplicationViewModel application, User user);
        List<BecomeAdminApplication> GetAllAdminApplicationsAsync();
    }
}
