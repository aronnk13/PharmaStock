using Microsoft.EntityFrameworkCore;
using pharmaStock.Core.DTO.Item;
using PharmaStock.Core.Interfaces.Repository;
using PharmaStock.Models;

namespace PharmaStock.Infrastructure.Repositories
{
    public class ItemRepository : IItemRepository
    {
        private readonly PharmaStockContext _context;

        public ItemRepository(PharmaStockContext context)
        {
            _context = context;
        }

        public async Task<Item?> GetByIdAsync(int itemId)
        {
            return await _context.Items
                .FirstOrDefaultAsync(i => i.ItemId == itemId);
        }

        public async System.Threading.Tasks.Task AddAsync(Item item)
        {
            _context.Items.Add(item);
            await _context.SaveChangesAsync();
        }

        public async System.Threading.Tasks.Task UpdateAsync(Item item)
        {
            _context.Items.Update(item);
            await _context.SaveChangesAsync();
        }
        public async Task<List<Item>> GetItemsFilteredAsync(ItemFilterDTO filter)
        {
            var query = _context.Items.AsQueryable();

            if (filter.PackSize.HasValue)
            {
                query = query.Where(i => i.PackSize == filter.PackSize.Value);
            }

            if (filter.IsActive.HasValue)
            {
                query = query.Where(i => i.Status == filter.IsActive.Value);
            }

            return await query.ToListAsync();
        }
    }
}