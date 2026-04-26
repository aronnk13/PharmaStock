using Microsoft.EntityFrameworkCore;
using PharmaStock.Core.DTO.Replenishment;
using PharmaStock.Core.Interfaces.Repository;
using PharmaStock.Core.Interfaces.Service;
using PharmaStock.Models;

namespace PharmaStock.Core.Services
{
    public class ReplenishmentService : IReplenishmentService
    {
        private readonly IReplenishmentRepository _repo;
        private readonly IInventoryBalanceRepository _balanceRepo;
        private readonly PharmaStockContext _context;

        public ReplenishmentService(
            IReplenishmentRepository repo,
            IInventoryBalanceRepository balanceRepo,
            PharmaStockContext context)
        {
            _repo = repo;
            _balanceRepo = balanceRepo;
            _context = context;
        }

        // ── Requests ────────────────────────────────────────────────────────────

        public async Task<IEnumerable<ReplenishmentRequestDTO>> GetAllRequestsAsync()
        {
            var items = await _repo.GetAllWithDetailsAsync();
            return items.Select(MapRequest);
        }

        public async Task<IEnumerable<ReplenishmentRequestDTO>> GetRequestsByStatusAsync(int status)
        {
            var items = await _repo.GetByStatusAsync(status);
            return items.Select(MapRequest);
        }

        public async Task<ReplenishmentRequestDTO?> GetRequestByIdAsync(int id)
        {
            var item = await _repo.GetByIdWithDetailsAsync(id);
            return item == null ? null : MapRequest(item);
        }

        public async Task<ReplenishmentRequestDTO> CreateRequestAsync(CreateReplenishmentRequestDTO dto)
        {
            var entity = new ReplenishmentRequest
            {
                LocationId = dto.LocationId,
                ItemId = dto.ItemId,
                SuggestedQty = dto.SuggestedQty,
                RuleId = dto.RuleId,
                CreatedDate = DateTime.UtcNow,
                Status = 1  // Open
            };
            await _repo.AddAsync(entity);
            var created = await _repo.GetByIdWithDetailsAsync(entity.ReplenishmentRequestId);
            return MapRequest(created ?? entity);
        }

        public async Task<bool> UpdateRequestStatusAsync(int id, int status)
        {
            var entity = await _repo.GetByIdAsync(id);
            if (entity == null) return false;
            entity.Status = status;
            return await _repo.UpdateAsync(entity);
        }

        // ── Rules ────────────────────────────────────────────────────────────────

        public async Task<IEnumerable<ReplenishmentRuleDTO>> GetAllRulesAsync()
        {
            var rules = await _repo.GetAllRulesAsync();
            return rules.Select(MapRule);
        }

        public async Task<ReplenishmentRuleDTO> CreateRuleAsync(CreateReplenishmentRuleDTO dto)
        {
            var entity = new ReplenishmentRule
            {
                LocationId = dto.LocationId,
                ItemId = dto.ItemId,
                MinLevel = dto.MinLevel,
                MaxLevel = dto.MaxLevel,
                ParLevel = dto.ParLevel,
                ReviewCycle = dto.ReviewCycle
            };
            await _repo.AddRuleAsync(entity);
            var created = await _repo.GetRuleByIdAsync(entity.ReplenishmentRuleId);
            return MapRule(created ?? entity);
        }

        public async Task<bool> UpdateRuleAsync(int id, CreateReplenishmentRuleDTO dto)
        {
            var entity = await _repo.GetRuleByIdAsync(id);
            if (entity == null) return false;
            entity.LocationId = dto.LocationId;
            entity.ItemId = dto.ItemId;
            entity.MinLevel = dto.MinLevel;
            entity.MaxLevel = dto.MaxLevel;
            entity.ParLevel = dto.ParLevel;
            entity.ReviewCycle = dto.ReviewCycle;
            return await _repo.UpdateRuleAsync(entity);
        }

        public async Task<bool> DeleteRuleAsync(int id) => await _repo.DeleteRuleAsync(id);

        // ── Auto Replenishment ───────────────────────────────────────────────────

        public async Task<RunCheckResultDTO> RunReplenishmentCheckAsync()
        {
            var rules  = await _repo.GetAllRulesAsync();
            int poCreated = 0;
            int skipped   = 0;
            var today  = DateOnly.FromDateTime(DateTime.Today);

            foreach (var rule in rules)
            {
                var balances = await _balanceRepo.GetByLocationAsync(rule.LocationId);
                var currentQty = balances
                    .Where(b => b.ItemId == rule.ItemId)
                    .Sum(b => b.QuantityOnHand - b.ReservedQty);

                // ── Stock sufficient → auto-close any stale open/converted requests ──
                if (currentQty >= rule.MinLevel)
                {
                    var stale = await _context.ReplenishmentRequests
                        .Where(r => r.LocationId == rule.LocationId
                                 && r.ItemId     == rule.ItemId
                                 && (r.Status == 1 || r.Status == 2))
                        .ToListAsync();
                    if (stale.Any())
                    {
                        stale.ForEach(r => r.Status = 3);
                        await _context.SaveChangesAsync();
                    }
                    continue;
                }

                // ── Stock still low — check if an active PO or manual TO is in progress ──
                var alreadyOpen = await _repo.HasOpenRequestAsync(rule.LocationId, rule.ItemId);
                if (alreadyOpen)
                {
                    var hasActiveTO = await _context.TransferOrders
                        .AnyAsync(to => to.ToLocationId == rule.LocationId
                                     && to.Status == 1
                                     && to.TransferItems.Any(ti => ti.ItemId == rule.ItemId));

                    var hasActivePO = await _context.PurchaseOrders
                        .AnyAsync(po => po.LocationId == rule.LocationId
                                     && po.PurchaseOrderStatusId != 3
                                     && po.PurchaseItems.Any(pi => pi.ItemId == rule.ItemId));

                    if (hasActiveTO || hasActivePO) { skipped++; continue; }

                    // Stale requests — close them and fall through to raise a fresh PO
                    var stale = await _context.ReplenishmentRequests
                        .Where(r => r.LocationId == rule.LocationId
                                 && r.ItemId     == rule.ItemId
                                 && (r.Status == 1 || r.Status == 2))
                        .ToListAsync();
                    stale.ForEach(r => r.Status = 3);
                    await _context.SaveChangesAsync();
                }

                // ── Create replenishment request ──
                var suggestedQty = rule.MaxLevel - currentQty;
                if (suggestedQty <= 0) suggestedQty = rule.MaxLevel;

                var request = new ReplenishmentRequest
                {
                    LocationId   = rule.LocationId,
                    ItemId       = rule.ItemId,
                    SuggestedQty = suggestedQty,
                    RuleId       = rule.ReplenishmentRuleId,
                    CreatedDate  = DateTime.UtcNow,
                    Status       = 1
                };
                await _repo.AddAsync(request);

                // ── Always raise a PO — TOs are manual only ──────────────────────────
                var vendor = await _context.PurchaseOrders
                    .Where(po => po.PurchaseItems.Any(pi => pi.ItemId == rule.ItemId))
                    .OrderByDescending(po => po.OrderDate)
                    .Select(po => po.Vendor)
                    .FirstOrDefaultAsync()
                    ?? await _context.Vendors.FirstOrDefaultAsync();

                if (vendor == null) continue; // No vendor configured — skip silently

                var po = new PharmaStock.Models.PurchaseOrder
                {
                    VendorId              = vendor.VendorId,
                    LocationId            = rule.LocationId,
                    OrderDate             = today,
                    ExpectedDate          = today.AddDays(7),
                    PurchaseOrderStatusId = 1  // Draft
                };
                _context.PurchaseOrders.Add(po);
                await _context.SaveChangesAsync();

                var lastPrice = await _context.PurchaseItems
                    .Where(pi => pi.ItemId == rule.ItemId && pi.UnitPrice > 0)
                    .OrderByDescending(pi => pi.PurchaseOrder.OrderDate)
                    .Select(pi => pi.UnitPrice)
                    .FirstOrDefaultAsync();

                _context.PurchaseItems.Add(new PharmaStock.Models.PurchaseItem
                {
                    PurchaseOrderId = po.PurchaseOrderId,
                    ItemId          = rule.ItemId,
                    OrderedQty      = suggestedQty,
                    UnitPrice       = lastPrice
                });
                await _context.SaveChangesAsync();

                request.Status = 2;
                await _repo.UpdateAsync(request);
                poCreated++;
            }

            return new RunCheckResultDTO
            {
                Message               = "Replenishment check complete",
                TransferOrdersCreated = 0,
                PurchaseOrdersCreated = poCreated,
                Skipped               = skipped
            };
        }

        public async Task<ConvertToTransferOrderResultDTO?> ConvertToTransferOrderAsync(int reqId)
        {
            var req = await _repo.GetByIdWithDetailsAsync(reqId);
            if (req == null) return null;
            if (req.Status != 1) return null;  // Only convert Open requests

            var today = DateOnly.FromDateTime(DateTime.Today);

            // ── Search ALL locations for available active non-expired stock ──────
            var allBalances = await _balanceRepo.GetAllWithDetailsAsync();
            var sourceBalance = allBalances
                .Where(b => b.ItemId == req.ItemId
                         && b.LocationId != req.LocationId          // not the requesting location itself
                         && (b.QuantityOnHand - b.ReservedQty) > 0
                         && b.InventoryLot != null
                         && b.InventoryLot.Status == 1              // Active
                         && b.InventoryLot.ExpiryDate > today)      // Not expired
                .OrderBy(b => b.InventoryLot!.ExpiryDate)           // FEFO
                .FirstOrDefault();

            req.Status = 2; // Mark as Converted regardless of outcome
            await _repo.UpdateAsync(req);

            // ── CASE 1: Stock found elsewhere → create Transfer Order ────────────
            if (sourceBalance != null)
            {
                var availableAtSource = sourceBalance.QuantityOnHand - sourceBalance.ReservedQty;
                var transferQty = Math.Min(req.SuggestedQty, availableAtSource);

                var order = new TransferOrder
                {
                    FromLocationId = sourceBalance.LocationId,
                    ToLocationId = req.LocationId,
                    CreatedDate = DateTime.UtcNow,
                    Status = 1  // Open
                };
                _context.TransferOrders.Add(order);
                await _context.SaveChangesAsync();

                _context.TransferItems.Add(new TransferItem
                {
                    TransferOrderId = order.TransferOrderId,
                    ItemId = req.ItemId,
                    InventoryLotId = sourceBalance.InventoryLotId,
                    Quantity = transferQty  // Capped at what source actually has
                });
                await _context.SaveChangesAsync();

                var fromLoc = await _context.Locations.FindAsync(sourceBalance.LocationId);
                var toLoc   = await _context.Locations.FindAsync(req.LocationId);

                return new ConvertToTransferOrderResultDTO
                {
                    ActionTaken         = "TransferOrder",
                    TransferOrderId     = order.TransferOrderId,
                    FromLocationId      = sourceBalance.LocationId,
                    FromLocationName    = fromLoc?.Name,
                    ToLocationId        = req.LocationId,
                    ToLocationName      = toLoc?.Name,
                    TransferItemCreated = true,
                    ReplenishmentRequestId = reqId,
                    ItemId       = req.ItemId,
                    ItemName     = req.Item?.Drug?.GenericName,
                    SuggestedQty = req.SuggestedQty,
                    Message      = $"Transfer Order created from {fromLoc?.Name} to {toLoc?.Name}."
                };
            }

            // ── CASE 2: No stock anywhere → create Purchase Order ────────────────
            // Find default vendor from most recent PO for this item, or first vendor
            var vendor = await _context.PurchaseOrders
                .Where(po => po.PurchaseItems.Any(pi => pi.ItemId == req.ItemId))
                .OrderByDescending(po => po.OrderDate)
                .Select(po => po.Vendor)
                .FirstOrDefaultAsync()
                ?? await _context.Vendors.FirstOrDefaultAsync();

            if (vendor == null)
            {
                return new ConvertToTransferOrderResultDTO
                {
                    ActionTaken = "Failed",
                    ReplenishmentRequestId = reqId,
                    ItemId = req.ItemId,
                    ItemName = req.Item?.Drug?.GenericName,
                    SuggestedQty = req.SuggestedQty,
                    Message = "No stock found anywhere and no vendor available to raise a PO."
                };
            }

            var po = new PharmaStock.Models.PurchaseOrder
            {
                VendorId    = vendor.VendorId,
                LocationId  = req.LocationId,
                OrderDate   = today,
                ExpectedDate = today.AddDays(7),
                PurchaseOrderStatusId = 1  // Draft
            };
            _context.PurchaseOrders.Add(po);
            await _context.SaveChangesAsync();

            // Pull last known unit price from previous POs for this item
            var lastPrice = await _context.PurchaseItems
                .Where(pi => pi.ItemId == req.ItemId && pi.UnitPrice > 0)
                .OrderByDescending(pi => pi.PurchaseOrder.OrderDate)
                .Select(pi => pi.UnitPrice)
                .FirstOrDefaultAsync();

            _context.PurchaseItems.Add(new PharmaStock.Models.PurchaseItem
            {
                PurchaseOrderId = po.PurchaseOrderId,
                ItemId          = req.ItemId,
                OrderedQty      = req.SuggestedQty,
                UnitPrice       = lastPrice  // 0 if no previous PO exists
            });
            await _context.SaveChangesAsync();

            var requestingLoc = await _context.Locations.FindAsync(req.LocationId);

            return new ConvertToTransferOrderResultDTO
            {
                ActionTaken    = "PurchaseOrder",
                PurchaseOrderId = po.PurchaseOrderId,
                VendorName     = vendor.Name,
                ReplenishmentRequestId = reqId,
                ItemId       = req.ItemId,
                ItemName     = req.Item?.Drug?.GenericName,
                SuggestedQty = req.SuggestedQty,
                Message      = $"No stock found anywhere. Purchase Order #{po.PurchaseOrderId} raised from vendor '{vendor.Name}' for {requestingLoc?.Name}."
            };
        }

        // ── Mappers ──────────────────────────────────────────────────────────────

        private static ReplenishmentRequestDTO MapRequest(ReplenishmentRequest r) => new()
        {
            ReplenishmentRequestId = r.ReplenishmentRequestId,
            LocationId = r.LocationId,
            LocationName = r.Location?.Name,
            ItemId = r.ItemId,
            ItemName = r.Item?.Drug?.GenericName,
            SuggestedQty = r.SuggestedQty,
            CreatedDate = r.CreatedDate,
            RuleId = r.RuleId,
            Status = r.Status,
            StatusName = r.StatusNavigation?.Status
        };

        private static ReplenishmentRuleDTO MapRule(ReplenishmentRule r) => new()
        {
            ReplenishmentRuleId = r.ReplenishmentRuleId,
            LocationId = r.LocationId,
            LocationName = r.Location?.Name,
            ItemId = r.ItemId,
            ItemName = r.Item?.Drug?.GenericName,
            MinLevel = r.MinLevel,
            MaxLevel = r.MaxLevel,
            ParLevel = r.ParLevel,
            ReviewCycle = r.ReviewCycle
        };
    }
}
