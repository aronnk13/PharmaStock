using Microsoft.EntityFrameworkCore;
using PharmaStock.Core.Interfaces.Repository;
using PharmaStock.Models;

namespace PharmaStock.Infrastructure.Repositories
{
    public class UserRepository(PharmaStockContext context) : GenericRepository<User>(context), IUserRepository
    {
        public async Task<bool> IsUserExistAsync(string username, string email, string phone, int? excludeUserId = null)
        {
            var query = context.Set<User>().AsQueryable();

            // If we are updating, ignore the user we are currently modifying
            if (excludeUserId.HasValue)
            {
                query = query.Where(u => u.UserId != excludeUserId.Value);
            }

            // Check if any remaining user has this exact username, email, or phone
            return await query.AnyAsync(u => 
                u.Username == username || 
                u.Email == email || 
                u.Phone == phone);
        }
    }
}