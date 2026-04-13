using PharmaStock.Models;

namespace PharmaStock.Core.Interfaces.Repository
{
    public interface IAuthRepository
    {
        Task<User?> GetUserByUsernameAsync(string username);
    }
}
