using Microsoft.EntityFrameworkCore;

using PharmaStock.Core.Interfaces.Repository;
using PharmaStock.Models;

namespace PharmaStock.Infrastructure.Repositories
{
    public class ItemRepository : GenericRepository<Item>, IItemRepository
    {
        private readonly PharmaStockContext _pharmaStockContext;
        public ItemRepository(PharmaStockContext pharmaStockContext)
             : base(pharmaStockContext)
        {
            _pharmaStockContext = pharmaStockContext;
        }
         public async Task<Item> GetItemByName(string itemName)
        {
            return await _pharmaStockContext.Items.FirstOrDefaultAsync(l => l.Drug.GenericName == itemName);
        }
    }
}