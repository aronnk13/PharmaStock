using PharmaStock.Models;

namespace PharmaStock.Core.Interfaces.Repository
{
    public interface IItemRepository : IGenericRepository<Item>
    {
        public Task<Item> GetItemByName(string itemName);
    }
}