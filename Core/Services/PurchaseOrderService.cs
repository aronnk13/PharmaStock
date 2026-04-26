using Microsoft.EntityFrameworkCore;
using PharmaStock.Core.DTO.GoodsReceipt;
using PharmaStock.Core.DTO.PurchaseOrder;
using PharmaStock.Core.Interfaces.Repository;
using PharmaStock.Core.Interfaces.Service;
using PharmaStock.Models;

namespace PharmaStock.Core.Services
{
    public class PurchaseOrderService : IPurchaseOrderService
    {
        private readonly IPurchaseOrderRepository _repo;
        private readonly PharmaStockContext _context;

        public PurchaseOrderService(IPurchaseOrderRepository repo, PharmaStockContext context)
        {
            _repo = repo;
            _context = context;
        }

        public async Task<IEnumerable<PurchaseOrderResponseDTO>> GetAllAsync()
        {
            return await _repo.GetAllWithDetailsAsync();
        }

        public async Task<PurchaseOrderResponseDTO?> GetByIdAsync(int id)
        {
            return await _repo.GetByIdWithDetailsAsync(id);
        }

        public async Task<IEnumerable<PurchaseOrderStatusDTO>> GetStatusesAsync()
        {
            return await _repo.GetAllStatusesAsync();
        }

        public async Task<PurchaseOrderResponseDTO> CreateAsync(CreatePurchaseOrderDTO dto)
        {
            if (dto.ExpectedDate < dto.OrderDate)
                throw new ArgumentException("Expected date cannot be before order date.");

            var defaultStatusId = await _repo.GetDefaultStatusIdAsync();

            var po = new PurchaseOrder
            {
                VendorId              = dto.VendorId,
                LocationId            = dto.LocationId,
                OrderDate             = dto.OrderDate,
                ExpectedDate          = dto.ExpectedDate,
                PurchaseOrderStatusId = defaultStatusId
            };

            await _repo.AddAsync(po);

            var result = await _repo.GetByIdWithDetailsAsync(po.PurchaseOrderId);
            return result!;
        }

        public async Task<PurchaseOrderResponseDTO> UpdateAsync(int id, UpdatePurchaseOrderDTO dto)
        {
            var po = await _repo.GetByIdAsync(id);
            if (po == null)
                throw new KeyNotFoundException("Purchase order not found.");

            if (dto.ExpectedDate.HasValue)
                po.ExpectedDate = dto.ExpectedDate.Value;

            if (dto.PurchaseOrderStatusId.HasValue)
            {
                var currentStatus = po.PurchaseOrderStatusId;
                var newStatus = dto.PurchaseOrderStatusId.Value;

                // Closed PO is final — cannot go back
                if (currentStatus == 3)
                    throw new InvalidOperationException("A Closed Purchase Order cannot be modified.");

                // Cannot approve/close without items
                if (newStatus >= 2)
                {
                    var hasItems = await _repo.HasItemsAsync(id);
                    if (!hasItems)
                        throw new InvalidOperationException("Cannot approve a Purchase Order with no items.");

                    // Cannot approve if any item has zero unit price
                    var hasZeroPrice = await _context.PurchaseItems
                        .AnyAsync(pi => pi.PurchaseOrderId == id && pi.UnitPrice <= 0);
                    if (hasZeroPrice)
                        throw new InvalidOperationException("Cannot approve a Purchase Order with items that have no unit price.");
                }

                po.PurchaseOrderStatusId = newStatus;
            }

            await _repo.UpdateAsync(po);

            var result = await _repo.GetByIdWithDetailsAsync(id);
            return result!;
        }

        public async System.Threading.Tasks.Task DeleteAsync(int id)
        {
            var po = await _repo.GetByIdAsync(id);
            if (po == null)
                throw new KeyNotFoundException("Purchase order not found.");

            // Only Draft POs can be deleted
            if (po.PurchaseOrderStatusId != 1)
                throw new InvalidOperationException("Only Draft purchase orders can be deleted.");

            // Check for GRNs — cannot delete if goods have been received
            var hasGrns = await _context.GoodsReciepts.AnyAsync(g => g.PurchaseOrderId == id);
            if (hasGrns)
                throw new InvalidOperationException("Cannot delete a purchase order that has goods receipts.");

            // Delete purchase items first to avoid FK violation
            var items = await _context.PurchaseItems.Where(i => i.PurchaseOrderId == id).ToListAsync();
            _context.PurchaseItems.RemoveRange(items);
            await _context.SaveChangesAsync();

            await _repo.DeleteAsync(id);
        }

        public async Task<List<ApprovedPendingGrnDTO>> GetApprovedPendingGrnAsync()
            => await _repo.GetApprovedPendingGrnAsync();

        public async Task<PoWithItemsDTO?> GetWithItemsAsync(int id)
            => await _repo.GetWithItemsAsync(id);
    }
}
