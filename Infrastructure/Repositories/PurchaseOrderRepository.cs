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
            // POs with an Open (awaiting QC) GRN cannot receive more stock yet
            var openGrnPoIds = await _context.GoodsReciepts
                .Where(g => g.StatusNavigation.Status == "Open")
                .Select(g => g.PurchaseOrderId)
                .Distinct()
                .ToListAsync();

            var receivableStatusIds = await _context.PurchaseOrderStatuses
                .Where(s => s.Status == "Approved" || s.Status == "PartiallyReceived")
                .Select(s => s.PurchaseOrderStatusId)
                .ToListAsync();

            return await _context.PurchaseOrders
                .Where(po => receivableStatusIds.Contains(po.PurchaseOrderStatusId)
                          && !openGrnPoIds.Contains(po.PurchaseOrderId))
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
            var po = await _context.PurchaseOrders
                .Include(p => p.Vendor)
                .Include(p => p.Location)
                .Include(p => p.PurchaseOrderStatus)
                .Include(p => p.PurchaseItems).ThenInclude(i => i.Item).ThenInclude(i => i.Drug)
                .FirstOrDefaultAsync(p => p.PurchaseOrderId == id);

            if (po == null) return null;

            // Sum accepted qty per itemId across all completed GRNs for this PO
            var acceptedByItem = await _context.GoodsReceiptItems
                .Where(g => g.GoodsReceipt.PurchaseOrderId == id)
                .GroupBy(g => g.ItemId)
                .Select(g => new { ItemId = g.Key, Accepted = g.Sum(x => x.AcceptedQty) })
                .ToListAsync();

            var acceptedMap = acceptedByItem.ToDictionary(x => x.ItemId, x => x.Accepted);

            return new PoWithItemsDTO
            {
                PurchaseOrderId = po.PurchaseOrderId,
                VendorId        = po.VendorId,
                VendorName      = po.Vendor.Name,
                LocationId      = po.LocationId,
                LocationName    = po.Location.Name,
                OrderDate       = po.OrderDate,
                ExpectedDate    = po.ExpectedDate,
                Status          = po.PurchaseOrderStatus.Status,
                Items = po.PurchaseItems.Select(i =>
                {
                    var accepted = acceptedMap.GetValueOrDefault(i.ItemId, 0);
                    return new PoItemDetailDTO
                    {
                        PurchaseItemId = i.PurchaseItemId,
                        ItemId         = i.ItemId,
                        ItemName       = i.Item.Drug.GenericName,
                        OrderedQty     = i.OrderedQty,
                        AcceptedQty    = accepted,
                        OutstandingQty = Math.Max(0, i.OrderedQty - accepted),
                        UnitPrice      = i.UnitPrice,
                        TaxPct         = i.TaxPct
                    };
                }).ToList()
            };
        }

        public Task<bool> HasItemsAsync(int purchaseOrderId) =>
            _context.PurchaseItems.AnyAsync(i => i.PurchaseOrderId == purchaseOrderId);
    }
}
