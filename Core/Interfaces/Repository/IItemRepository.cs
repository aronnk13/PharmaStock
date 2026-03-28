using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PharmaStock.Models;

namespace PharmaStock.Core.Interfaces.Repository
{

    public interface IItemRepository
    {
        Task<Item?> GetByIdAsync(int itemId);
        System.Threading.Tasks.Task AddAsync(Item item);
        System.Threading.Tasks.Task UpdateAsync(Item item);
    }

}