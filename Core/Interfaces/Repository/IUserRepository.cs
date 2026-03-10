using PharmaStock.Models;

namespace PharmaStock.Core.Interfaces.Repository
{
    public interface IUserRepository : IGenericRepository<User>
    {
        //any specific methods (like get user by role) will be declared here.
    }
}