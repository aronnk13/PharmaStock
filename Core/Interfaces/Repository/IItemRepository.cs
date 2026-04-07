using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using pharmaStock.Core.DTO.Item;
using PharmaStock.Models;

namespace PharmaStock.Core.Interfaces.Repository
{
    public interface IItemRepository : IGenericRepository<Item>
    {
        public Task<Item> GetItemByName(string itemName);
    }
}


    public interface IItemRepository
    {
        Task<Item?> GetByIdAsync(int itemId);
        System.Threading.Tasks.Task AddAsync(Item item);
        System.Threading.Tasks.Task UpdateAsync(Item item);
        Task<List<Item>> GetItemsFilteredAsync(ItemFilterDTO filter);
        Task<ItemDeletedResponseDTO> DeleteItem(int itemId);
    }

}
