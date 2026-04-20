using Microsoft.EntityFrameworkCore;
using PharmaStock.Core.Interfaces.Repository;
using PharmaStock.Models;

namespace PharmaStock.Infrastructure.Repositories
{
    public class AuthRepository : IAuthRepository
    {
        private readonly PharmaStockContext _context;

        public AuthRepository(PharmaStockContext context)
        {
            _context = context;
        }

        public async Task<User?> GetUserByUsernameAsync(string username)
        {
            return await _context.Users
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.Username == username);
        }
    }
}
