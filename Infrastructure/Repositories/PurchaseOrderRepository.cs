using Microsoft.EntityFrameworkCore;
using PharmaStock.Core.DTO.GoodsReceipt;
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
            var first = await _context.PurchaseOrderStatuses
                .OrderBy(s => s.PurchaseOrderStatusId)
                .FirstOrDefaultAsync();
            return first?.PurchaseOrderStatusId ?? 1;
        }

        public async Task<List<ApprovedPendingGrnDTO>> GetApprovedPendingGrnAsync()
        {
            var approvedStatusIds = await _context.PurchaseOrderStatuses
                .Where(s => s.Status == "Approved")
                .Select(s => s.PurchaseOrderStatusId)
                .ToListAsync();

            var poIdsWithGrn = await _context.GoodsReciepts
                .Select(g => g.PurchaseOrderId)
                .Distinct()
                .ToListAsync();

            return await _context.PurchaseOrders
                .Where(po => approvedStatusIds.Contains(po.PurchaseOrderStatusId)
                          && !poIdsWithGrn.Contains(po.PurchaseOrderId))
                .Select(po => new ApprovedPendingGrnDTO
                {
                    PurchaseOrderId = po.PurchaseOrderId,
                    VendorName      = po.Vendor.Name,
                    LocationId      = po.LocationId,
                    LocationName    = po.Location.Name,
                    OrderDate       = po.OrderDate,
                    ExpectedDate    = po.ExpectedDate,
                    Status          = po.PurchaseOrderStatus.Status,
                    ItemCount       = po.PurchaseItems.Count()
                })
                .OrderBy(po => po.ExpectedDate)
                .ToListAsync();
        }

        public async Task<PoWithItemsDTO?> GetWithItemsAsync(int id)
        {
            return await _context.PurchaseOrders
                .Where(po => po.PurchaseOrderId == id)
                .Select(po => new PoWithItemsDTO
                {
                    PurchaseOrderId = po.PurchaseOrderId,
                    VendorId        = po.VendorId,
                    VendorName      = po.Vendor.Name,
                    LocationId      = po.LocationId,
                    LocationName    = po.Location.Name,
                    OrderDate       = po.OrderDate,
                    ExpectedDate    = po.ExpectedDate,
                    Status          = po.PurchaseOrderStatus.Status,
                    Items = po.PurchaseItems.Select(i => new PoItemDetailDTO
                    {
                        PurchaseItemId = i.PurchaseItemId,
                        ItemId         = i.ItemId,
                        ItemName       = i.Item.Drug.GenericName,
                        OrderedQty     = i.OrderedQty,
                        UnitPrice      = i.UnitPrice,
                        TaxPct         = i.TaxPct
                    }).ToList()
                })
                .FirstOrDefaultAsync();
        }
    }
}
