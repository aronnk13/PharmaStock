using Microsoft.EntityFrameworkCore;
using PharmaStock.Core.Interfaces.Repository;
using PharmaStock.Models;

namespace PharmaStock.Infrastructure.Repositories
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        private readonly PharmaStockContext _pharmaStockContext;

        public UserRepository(PharmaStockContext pharmaStockContext)
            : base(pharmaStockContext)
        {
            _pharmaStockContext = pharmaStockContext;
        }

        public async Task<bool> IsUserExistAsync(string username, string email, string phone)
        {
            return await _pharmaStockContext.Users.AnyAsync(u =>
                u.Username == username ||
                u.Email == email ||
                u.Phone == phone);
        }

        public async Task<IEnumerable<User>> GetAllUsersWithRoleAsync()
        {
            return await _pharmaStockContext.Users
                .Include(u => u.Role)
                .ToListAsync();
        }

        public async Task<User?> GetUserByIdWithRoleAsync(int id)
        {
            return await _pharmaStockContext.Users
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.UserId == id);
        }

        public async Task<IEnumerable<UserRole>> GetAllRolesAsync()
        {
            return await _pharmaStockContext.UserRoles.ToListAsync();
        }

        public async Task<UserRole?> GetRoleByIdAsync(int id)
        {
            return await _pharmaStockContext.UserRoles.FindAsync(id);
        }
    }
}