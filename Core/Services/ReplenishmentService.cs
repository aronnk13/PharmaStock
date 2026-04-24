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
            var rules = await _repo.GetAllRulesAsync();
            int created = 0;

            foreach (var rule in rules)
            {
                // Sum available qty (on-hand minus reserved) across all bins at this location
                var balances = await _balanceRepo.GetByLocationAsync(rule.LocationId);
                var currentQty = balances
                    .Where(b => b.ItemId == rule.ItemId)
                    .Sum(b => b.QuantityOnHand - b.ReservedQty);

                if (currentQty < rule.MinLevel)
                {
                    var alreadyOpen = await _repo.HasOpenRequestAsync(rule.LocationId, rule.ItemId);
                    if (!alreadyOpen)
                    {
                        var suggestedQty = rule.MaxLevel - currentQty;
                        if (suggestedQty <= 0) suggestedQty = rule.MaxLevel;

                        await _repo.AddAsync(new ReplenishmentRequest
                        {
                            LocationId = rule.LocationId,
                            ItemId = rule.ItemId,
                            SuggestedQty = suggestedQty,
                            RuleId = rule.ReplenishmentRuleId,
                            CreatedDate = DateTime.UtcNow,
                            Status = 1  // Open
                        });
                        created++;
                    }
                }
            }

            return new RunCheckResultDTO
            {
                Message = "Replenishment check complete",
                NewRequestsCreated = created
            };
        }

        public async Task<ConvertToTransferOrderResultDTO?> ConvertToTransferOrderAsync(
            int reqId, int fromLocationId = 1)
        {
            var req = await _repo.GetByIdWithDetailsAsync(reqId);
            if (req == null) return null;
            if (req.Status != 1) return null;  // Only convert Open requests

            // Create the TransferOrder
            var order = new TransferOrder
            {
                FromLocationId = fromLocationId,
                ToLocationId = req.LocationId,
                CreatedDate = DateTime.UtcNow,
                Status = 1  // Open
            };
            _context.TransferOrders.Add(order);
            await _context.SaveChangesAsync();

            // FEFO: pick earliest-expiry lot with available stock at source location
            var balances = await _balanceRepo.GetByLocationAsync(fromLocationId);
            var fefoBalance = balances
                .Where(b => b.ItemId == req.ItemId && (b.QuantityOnHand - b.ReservedQty) > 0)
                .OrderBy(b => b.InventoryLot?.ExpiryDate)
                .FirstOrDefault();

            bool itemCreated = false;
            if (fefoBalance != null)
            {
                _context.TransferItems.Add(new TransferItem
                {
                    TransferOrderId = order.TransferOrderId,
                    ItemId = req.ItemId,
                    InventoryLotId = fefoBalance.InventoryLotId,
                    Quantity = req.SuggestedQty
                });
                await _context.SaveChangesAsync();
                itemCreated = true;
            }

            // Mark request as Converted (status 2)
            req.Status = 2;
            await _repo.UpdateAsync(req);

            // Reload to get nav props for the response
            var fromLocation = await _context.Locations.FindAsync(fromLocationId);
            var toLocation = await _context.Locations.FindAsync(req.LocationId);

            return new ConvertToTransferOrderResultDTO
            {
                TransferOrderId = order.TransferOrderId,
                FromLocationId = fromLocationId,
                FromLocationName = fromLocation?.Name,
                ToLocationId = req.LocationId,
                ToLocationName = toLocation?.Name,
                CreatedDate = order.CreatedDate,
                Status = order.Status,
                ReplenishmentRequestId = reqId,
                ItemId = req.ItemId,
                ItemName = req.Item?.Drug?.GenericName,
                SuggestedQty = req.SuggestedQty,
                TransferItemCreated = itemCreated
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
