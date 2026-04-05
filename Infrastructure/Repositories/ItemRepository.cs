using Microsoft.EntityFrameworkCore;
using pharmaStock.Core.DTO.Item;
using PharmaStock.Core.DTO;
using PharmaStock.Core.Interfaces;
using PharmaStock.Core.Interfaces.Repository;
using PharmaStock.Models;

namespace PharmaStock.Infrastructure.Repositories
{
    public class ItemRepository : IItemRepository
    {
        private readonly PharmaStockContext _context;
        private readonly IAuditLogService _auditLogService;

        public ItemRepository(PharmaStockContext context, IAuditLogService auditLogService)
        {
            _context = context;
            _auditLogService = auditLogService;
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
        public async Task<ItemDeletedResponseDTO> DeleteItem(int itemId, int requestingUserId)
        {
            try
            {
                var item = await _context.Items.FindAsync(itemId);

                if (item == null)
                {
                    return new ItemDeletedResponseDTO
                    {
                        IsDeleted = false,
                        Message = "Item not found."
                    };
                }
                // Soft delete
                item.Status = false;
                _context.Items.Update(item);

                var rowsAffected = await _context.SaveChangesAsync();

                if (rowsAffected > 0)
                {
                    await _auditLogService.CreateLogAsync(new AuditDto
                    {
                        UserId = requestingUserId,
                        Action = "INACTIVATE",
                        Resource = "Item",
                        Metadata = $"{{\"itemId\":{itemId}}}"
                    });
                    return new ItemDeletedResponseDTO
                    {
                        IsDeleted = true,
                        Message = "Item deleted successfully."
                    };
                }

                return new ItemDeletedResponseDTO
                {
                    IsDeleted = false,
                    Message = "Delete failed: No changes were saved to the database."
                };
            }
            catch (Exception ex)
            {
                return new ItemDeletedResponseDTO
                {
                    IsDeleted = false,
                    Message = $"An error occurred: {ex.Message}"
                };
            }
        }
    }
}