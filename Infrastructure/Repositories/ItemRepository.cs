using Microsoft.EntityFrameworkCore;
using pharmaStock.Core.DTO.Item;
using PharmaStock.Core.DTO.Item;
using PharmaStock.Core.Interfaces.Repository;
using PharmaStock.Models;
using Task = System.Threading.Tasks.Task;

namespace PharmaStock.Infrastructure.Repositories
{
    public class ItemRepository : IItemRepository
    {
        private readonly PharmaStockContext _context;

        public ItemRepository(PharmaStockContext context)
        {
            _context = context;
        }

        public async Task<GetItemDTO?> GetItemDtoByIdAsync(int itemId)
        {
            return await _context.Items
                .Where(i => i.ItemId == itemId)
                .Select(i => new GetItemDTO
                {
                    ItemId = i.ItemId,
                    DrugId = i.DrugId,
                    DrugName = i.Drug.GenericName,
                    PackSize = i.PackSize,
                    UoMId = i.UoM,
                    UoMCode = i.UoMNavigation.Code,
                    ConversionToEach = i.ConversionToEach,
                    Barcode = i.Barcode,
                    Status = i.Status
                })
                .FirstOrDefaultAsync();
        }

        public async Task<List<GetItemDTO>> GetAllAsync()
        {
            return await _context.Items
                .Select(i => new GetItemDTO
                {
                    ItemId = i.ItemId,
                    DrugId = i.DrugId,
                    DrugName = i.Drug.GenericName,
                    PackSize = i.PackSize,
                    UoMId = i.UoM,
                    UoMCode = i.UoMNavigation.Code,
                    ConversionToEach = i.ConversionToEach,
                    Barcode = i.Barcode,
                    Status = i.Status
                })
                .ToListAsync();
        }

        public async Task<Item?> GetByIdAsync(int itemId)
        {
            return await _context.Items.FindAsync(itemId);
        }

        public async Task AddAsync(Item item)
        {
            _context.Items.Add(item);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Item item)
        {
            _context.Items.Update(item);
            await _context.SaveChangesAsync();
        }

        public async Task<ItemDeletedResponseDTO> DeleteItem(int itemId)
        {
            try
            {
                var item = await _context.Items.FindAsync(itemId);
                if (item == null)
                    return new ItemDeletedResponseDTO { IsDeleted = false, Message = "Item not found." };

                if (await _context.InventoryLots.AnyAsync(x => x.ItemId == itemId))
                    return new ItemDeletedResponseDTO { IsDeleted = false, Message = "Cannot delete this item. It has inventory lots linked to it." };

                if (await _context.GoodsReceiptItems.AnyAsync(x => x.ItemId == itemId))
                    return new ItemDeletedResponseDTO { IsDeleted = false, Message = "Cannot delete this item. It has goods receipt records linked to it." };

                if (await _context.PurchaseItems.AnyAsync(x => x.ItemId == itemId))
                    return new ItemDeletedResponseDTO { IsDeleted = false, Message = "Cannot delete this item. It is referenced in purchase orders." };

                _context.Items.Remove(item);
                var rowsAffected = await _context.SaveChangesAsync();

                if (rowsAffected > 0)
                    return new ItemDeletedResponseDTO { IsDeleted = true, Message = "Item deleted successfully." };

                return new ItemDeletedResponseDTO { IsDeleted = false, Message = "Delete failed: no changes saved." };
            }
            catch (Exception ex)
            {
                return new ItemDeletedResponseDTO { IsDeleted = false, Message = $"An error occurred: {ex.Message}" };
            }
        }

        public async Task<bool> ToggleStatusAsync(int itemId)
        {
            var item = await _context.Items.FindAsync(itemId);
            if (item == null) return false;
            item.Status = !item.Status;
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
