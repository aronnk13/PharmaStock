using System;
using PharmaStock.Core.Interfaces.Repository;
using PharmaStock.Models;

namespace PharmaStock.Infrastructure.Repositories
{
    public class UserRepository(PharmaStockContext context) : GenericRepository<User>(context), IUserRepository
    {
    }
}