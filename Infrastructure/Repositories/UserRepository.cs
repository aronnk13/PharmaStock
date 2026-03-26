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
            var query = _pharmaStockContext.Set<User>().AsQueryable();
            // Check if any remaining user has this exact username, email, or phone
            return await query.AnyAsync(u => 
                u.Username == username || 
                u.Email == email || 
                u.Phone == phone);
        }
    }
}