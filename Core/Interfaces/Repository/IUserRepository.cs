using System;
using PharmaStock.Models;
namespace PharmaStock.Core.Interfaces.Repository
{
    public interface IUserRepository : IGenericRepository<User>
    {
        Task<bool> IsUserExistAsync(string username, string email, string phone, int? excludeUserId = null);
    }
}