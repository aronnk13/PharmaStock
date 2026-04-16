using PharmaStock.Models;

namespace PharmaStock.Core.Interfaces.Repository
{
    public interface IUserRepository : IGenericRepository<User>
    {
        Task<bool> IsUserExistAsync(string username, string email, string phone);
        Task<IEnumerable<User>> GetAllUsersWithRoleAsync();
        Task<User?> GetUserByIdWithRoleAsync(int id);
        Task<IEnumerable<UserRole>> GetAllRolesAsync();
        Task<UserRole?> GetRoleByIdAsync(int id);
    }
}