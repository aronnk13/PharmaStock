using System;
using PharmaStock.Models;
using PharmaStock.Core.Interfaces;

namespace PharmaStock.Infrastructure.Repositories
{
    public class UserRepository : GenericRepository<User>
    {
        public UserRepository(PharmaStockContext context) : base(context)
        {
        }

        //any specific methosd (like get user by role) will be implemented here.
    }
}