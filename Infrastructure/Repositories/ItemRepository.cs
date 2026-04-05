using Microsoft.EntityFrameworkCore;
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
        public async Task<List<Item>> GetItemsAsync(int? drugId = null)
        {
            var query = _context.Items.AsQueryable();

            if (drugId.HasValue)
            {
                query = query.Where(i => i.DrugId == drugId.Value);
            }

            return await query.ToListAsync();
        }
    }
}