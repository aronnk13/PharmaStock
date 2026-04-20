using PharmaStock.Core.DTO.Replenishment;
using PharmaStock.Core.Interfaces.Repository;
using PharmaStock.Core.Interfaces.Service;
using PharmaStock.Models;

namespace PharmaStock.Core.Services
{
    public class ReplenishmentService : IReplenishmentService
    {
        private readonly IReplenishmentRepository _repo;

        public ReplenishmentService(IReplenishmentRepository repo)
        {
            _repo = repo;
        }

        public async Task<IEnumerable<ReplenishmentRequestDTO>> GetAllRequestsAsync()
        {
            var items = await _repo.GetAllAsync();
            return items.Select(Map);
        }

        public async Task<IEnumerable<ReplenishmentRequestDTO>> GetRequestsByStatusAsync(int status)
        {
            var items = await _repo.GetByStatusAsync(status);
            return items.Select(Map);
        }

        public async Task<ReplenishmentRequestDTO?> GetRequestByIdAsync(int id)
        {
            var item = await _repo.GetByIdAsync(id);
            return item == null ? null : Map(item);
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
                Status = 1
            };
            await _repo.AddAsync(entity);
            return Map(entity);
        }

        public async Task<bool> UpdateRequestStatusAsync(int id, int status)
        {
            var entity = await _repo.GetByIdAsync(id);
            if (entity == null) return false;
            entity.Status = status;
            return await _repo.UpdateAsync(entity);
        }

        public async Task<IEnumerable<ReplenishmentRuleDTO>> GetAllRulesAsync()
        {
            var rules = await _repo.GetAllRulesAsync();
            return rules.Select(r => new ReplenishmentRuleDTO
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
            });
        }

        private static ReplenishmentRequestDTO Map(ReplenishmentRequest r) => new()
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
    }
}
