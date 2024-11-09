using System.Threading.Tasks;
using misticProject.Data.Models;

namespace misticProject.Interface
{
    public interface IUserRepository
    {
        Task<AppUser> GetUserByIdAsync(string userId);
        Task<AppUser> GetUserByEmailAsync(string email);
        Task<bool> CreateUserAsync(AppUser user, string password);
        Task<bool> UpdateUserAsync(AppUser user);
        Task<bool> DeleteUserAsync(string userId);
    }
}
