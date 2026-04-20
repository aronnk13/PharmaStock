using Microsoft.EntityFrameworkCore;
using PharmaStock.Core.DTO.PurchaseOrder;
using PharmaStock.Core.Interfaces.Repository;
using PharmaStock.Models;

namespace PharmaStock.Infrastructure.Repositories
{
    public class PurchaseOrderRepository : GenericRepository<PurchaseOrder>, IPurchaseOrderRepository
    {
        private readonly PharmaStockContext _context;

        public PurchaseOrderRepository(PharmaStockContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<PurchaseOrderResponseDTO>> GetAllWithDetailsAsync()
        {
            return await _context.PurchaseOrders
                .Include(po => po.Vendor)
                .Include(po => po.PurchaseOrderStatus)
                .Select(po => new PurchaseOrderResponseDTO
                {
                    PurchaseOrderId       = po.PurchaseOrderId,
                    VendorId              = po.VendorId,
                    VendorName            = po.Vendor.Name,
                    LocationId            = po.LocationId,
                    OrderDate             = po.OrderDate,
                    ExpectedDate          = po.ExpectedDate,
                    PurchaseOrderStatusId = po.PurchaseOrderStatusId,
                    Status                = po.PurchaseOrderStatus.Status
                })
                .ToListAsync();
        }

        public async Task<PurchaseOrderResponseDTO?> GetByIdWithDetailsAsync(int id)
        {
            return await _context.PurchaseOrders
                .Include(po => po.Vendor)
                .Include(po => po.PurchaseOrderStatus)
                .Where(po => po.PurchaseOrderId == id)
                .Select(po => new PurchaseOrderResponseDTO
                {
                    PurchaseOrderId       = po.PurchaseOrderId,
                    VendorId              = po.VendorId,
                    VendorName            = po.Vendor.Name,
                    LocationId            = po.LocationId,
                    OrderDate             = po.OrderDate,
                    ExpectedDate          = po.ExpectedDate,
                    PurchaseOrderStatusId = po.PurchaseOrderStatusId,
                    Status                = po.PurchaseOrderStatus.Status
                })
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<PurchaseOrderStatusDTO>> GetAllStatusesAsync()
        {
            return await _context.PurchaseOrderStatuses
                .Select(s => new PurchaseOrderStatusDTO
                {
                    PurchaseOrderStatusId = s.PurchaseOrderStatusId,
                    Status                = s.Status
                })
                .ToListAsync();
        }

        public async Task<int> GetDefaultStatusIdAsync()
        {
            // Return the first status (typically Draft/Pending)
            var first = await _context.PurchaseOrderStatuses
                .OrderBy(s => s.PurchaseOrderStatusId)
                .FirstOrDefaultAsync();
            return first?.PurchaseOrderStatusId ?? 1;
        }
    }
}
