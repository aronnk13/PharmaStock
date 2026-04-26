using Microsoft.EntityFrameworkCore;
using PharmaStock.Core.DTO.GoodsReceipt;
using PharmaStock.Core.Interfaces;
using PharmaStock.Core.Interfaces.Repository;
using PharmaStock.Models;

namespace PharmaStock.Infrastructure.Repositories
{
    public class GrnRepository : GenericRepository<GoodsReciept>, IGrnRepository
    {
        private readonly PharmaStockContext _context;

        public GrnRepository(PharmaStockContext context) : base(context)
        {
            _context = context;
        }

        public async Task<PurchaseOrder?> GetPurchaseOrderByIdAsync(int purchaseOrderId)
        {
            return await _context.PurchaseOrders
                .Include(po => po.PurchaseOrderStatus)
                .FirstOrDefaultAsync(po => po.PurchaseOrderId == purchaseOrderId);
        }

        public async Task<bool> HasGrnItemsAsync(int grnId)
        {
            return await _context.GoodsReceiptItems
                .AnyAsync(i => i.GoodsReceiptId == grnId);
        }

        public async Task<GetGrnDTO?> GetGrnDtoByIdAsync(int grnId)
        {
            return await _context.GoodsReciepts
                .Where(g => g.GoodsRecieptId == grnId)
                .Select(g => new GetGrnDTO
                {
                    GoodsReceiptId = g.GoodsRecieptId,
                    PurchaseOrderId = g.PurchaseOrderId,
                    VendorName = g.PurchaseOrder.Vendor.Name,
                    ReceivedDate = g.ReceivedDate,
                    StatusId = g.Status,
                    Status = g.StatusNavigation.Status,
                    ReceivedBy = g.ReceivedBy,
                    ItemCount = g.GoodsReceiptItems.Count()
                })
                .FirstOrDefaultAsync();
        }

        public async Task<GetGrnWithItemsDTO?> GetGrnWithItemsAsync(int grnId)
        {
            Console.WriteLine($"[GRN-REPO] GetGrnWithItemsAsync called for GRN {grnId}");

            // Load entity with all navigation properties explicitly to avoid
            // silent empty-collection issues with deep Select projections.
            var grn = await _context.GoodsReciepts
                .Include(g => g.GoodsReceiptItems)
                    .ThenInclude(i => i.Item)
                        .ThenInclude(item => item.Drug)
                .Include(g => g.GoodsReceiptItems)
                    .ThenInclude(i => i.PurchaseOrderItem)
                .Include(g => g.PurchaseOrder)
                    .ThenInclude(po => po.Vendor)
                .Include(g => g.PurchaseOrder)
                    .ThenInclude(po => po.Location)
                .Include(g => g.StatusNavigation)
                .AsSplitQuery()
                .FirstOrDefaultAsync(g => g.GoodsRecieptId == grnId);

            if (grn == null)
            {
                Console.WriteLine($"[GRN-REPO] GRN {grnId} not found");
                return null;
            }

            Console.WriteLine($"[GRN-REPO] GRN {grnId} loaded — {grn.GoodsReceiptItems.Count} item(s)");

            return new GetGrnWithItemsDTO
            {
                GoodsReceiptId = grn.GoodsRecieptId,
                PurchaseOrderId = grn.PurchaseOrderId,
                VendorName = grn.PurchaseOrder?.Vendor?.Name,
                LocationName = grn.PurchaseOrder?.Location?.Name ?? string.Empty,
                ReceivedDate = grn.ReceivedDate,
                StatusId = grn.Status,
                Status = grn.StatusNavigation?.Status ?? string.Empty,
                ReceivedBy = grn.ReceivedBy,
                ItemCount = grn.GoodsReceiptItems.Count,
                Items = grn.GoodsReceiptItems.Select(i => new GrnItemDetailDTO
                {
                    GoodsReceiptItemId = i.GoodsReceiptItemId,
                    ItemId = i.ItemId,
                    ItemName = i.Item?.Drug?.GenericName ?? "Unknown",
                    BatchNumber = i.BatchNumber.ToString(),   // int entity → string DTO
                    ExpiryDate = i.ExpiryDate,
                    OrderedQty = i.PurchaseOrderItem?.OrderedQty ?? 0,
                    ReceivedQty = i.ReceivedQty,
                    AcceptedQty = i.AcceptedQty,
                    RejectedQty = i.RejectedQty,
                    RejectionReason = i.Reason
                }).ToList()
            };
        }

        public async Task<List<GetGrnDTO>> GetPendingQcGrnsAsync()
        {
            // Open status = 1 (OPEN), look up dynamically
            var openStatusIds = await _context.GoodsReceiptStatuses
                .Where(s => s.Status == "Open")
                .Select(s => s.GoodsReceiptStatusId)
                .ToListAsync();

            return await _context.GoodsReciepts
                .Where(g => openStatusIds.Contains(g.Status))
                .Select(g => new GetGrnDTO
                {
                    GoodsReceiptId = g.GoodsRecieptId,
                    PurchaseOrderId = g.PurchaseOrderId,
                    VendorName = g.PurchaseOrder.Vendor.Name,
                    ReceivedDate = g.ReceivedDate,
                    StatusId = g.Status,
                    Status = g.StatusNavigation.Status,
                    ReceivedBy = g.ReceivedBy,
                    ItemCount = g.GoodsReceiptItems.Count()
                })
                .OrderByDescending(g => g.ReceivedDate)
                .ToListAsync();
        }

        public async Task<List<int>> GetReceivablePoStatusIdsAsync()
        {
            return await _context.PurchaseOrderStatuses
                .Where(s => s.Status == "Approved" || s.Status == "PartiallyReceived")
                .Select(s => s.PurchaseOrderStatusId)
                .ToListAsync();
        }

        public async Task<string?> GetUsernameByIdAsync(int userId)
        {
            return await _context.Users
                .Where(u => u.UserId == userId)
                .Select(u => u.Username)
                .FirstOrDefaultAsync();
        }

        public async Task<GoodsReceiptStatus?> GetGrnStatusByCodeAsync(string statusCode)
        {
            return await _context.GoodsReceiptStatuses
                .FirstOrDefaultAsync(s => s.Status == statusCode);
        }

        public async Task<(List<GetGrnDTO>, int)> GetGrnsByFilterAsync(GrnFilterDTO filter)
        {
            var query = _context.GoodsReciepts.AsQueryable();

            if (filter.StatusId.HasValue)
                query = query.Where(g => g.Status == filter.StatusId.Value);

            if (filter.FromDate.HasValue)
                query = query.Where(g => g.ReceivedDate >= filter.FromDate.Value);

            if (filter.ToDate.HasValue)
                query = query.Where(g => g.ReceivedDate <= filter.ToDate.Value);

            if (!string.IsNullOrEmpty(filter.ReceivedBy))
                query = query.Where(g => g.ReceivedBy == filter.ReceivedBy);

            var totalCount = await query.CountAsync();

            var pageSize = filter.PageSize > 100 ? 100 : filter.PageSize;

            var grns = await query
                .OrderByDescending(g => g.ReceivedDate)
                .Skip((filter.Page - 1) * pageSize)
                .Take(pageSize)
                .Select(g => new GetGrnDTO
                {
                    GoodsReceiptId = g.GoodsRecieptId,
                    PurchaseOrderId = g.PurchaseOrderId,
                    VendorName = g.PurchaseOrder.Vendor.Name,
                    ReceivedDate = g.ReceivedDate,
                    StatusId = g.Status,
                    Status = g.StatusNavigation.Status,
                    ReceivedBy = g.ReceivedBy,
                    ItemCount = g.GoodsReceiptItems.Count()
                })
                .ToListAsync();

            return (grns, totalCount);
        }

        // ── CompleteQc lookup helpers ─────────────────────────────────────────────

        public async Task<int?> GetInventoryLotStatusIdByNameAsync(string statusName)
        {
            return await _context.InventoryLotStatuses
                .Where(s => s.Status == statusName)
                .Select(s => (int?)s.InventoryLotStatusId)
                .FirstOrDefaultAsync();
        }

        public async Task<int?> GetPoStatusIdByNameAsync(string statusName)
        {
            return await _context.PurchaseOrderStatuses
                .Where(s => s.Status == statusName)
                .Select(s => (int?)s.PurchaseOrderStatusId)
                .FirstOrDefaultAsync();
        }

        public async Task<int?> GetStockTransitionTypeIdByNameAsync(string typeName)
        {
            return await _context.StockTransitionTypes
                .Where(s => s.TransitionType == typeName)
                .Select(s => (int?)s.StockTransitionTypeId)
                .FirstOrDefaultAsync();
        }

        public async Task<int?> GetFirstBinIdAtLocationAsync(int locationId)
        {
            return await _context.Bins
                .Where(b => b.LocationId == locationId)
                .Select(b => (int?)b.BinId)
                .FirstOrDefaultAsync();
        }

        public async Task<GoodsReciept?> GetGrnWithPoAndItemsAsync(int grnId)
        {
            return await _context.GoodsReciepts
                .Include(g => g.PurchaseOrder)
                    .ThenInclude(po => po.PurchaseItems)
                .Include(g => g.GoodsReceiptItems)
                .Include(g => g.StatusNavigation)
                .FirstOrDefaultAsync(g => g.GoodsRecieptId == grnId);
        }
    }
}
