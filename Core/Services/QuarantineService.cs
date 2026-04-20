using PharmaStock.Core.DTO.QCO;
using PharmaStock.Core.Interfaces.Repository;
using PharmaStock.Core.Interfaces.Service;
using PharmaStock.Models;

namespace PharmaStock.Core.Services
{
    public class QuarantineService : IQuarantineService
    {
        private readonly IQuarantineRepository _repo;

        public QuarantineService(IQuarantineRepository repo) => _repo = repo;

        public async Task<IEnumerable<QuarantineActionDTO>> GetAllAsync()
        {
            var items = await _repo.GetAllWithDetailsAsync();
            return items.Select(Map);
        }

        public async Task<QuarantineActionDTO?> GetByIdAsync(int id)
        {
            var item = await _repo.GetByIdWithDetailsAsync(id);
            return item == null ? null : Map(item);
        }

        public async Task<QuarantineActionDTO> CreateAsync(CreateQuarantineActionDTO dto)
        {
            var entity = new QuarantaineAction
            {
                InventoryLotId = dto.InventoryLotId,
                Reason = dto.Reason,
                QuarantineDate = DateTime.UtcNow,
                Status = 1
            };
            await _repo.AddAsync(entity);
            var created = await _repo.GetByIdWithDetailsAsync(entity.QuarantaineActionId);
            return Map(created ?? entity);
        }

        public async Task<bool> ReleaseAsync(int id)
        {
            var entity = await _repo.GetByIdAsync(id);
            if (entity == null) return false;
            entity.ReleasedDate = DateTime.UtcNow;
            entity.Status = 2;
            return await _repo.UpdateAsync(entity);
        }

        public async Task<bool> DisposeAsync(int id)
        {
            var entity = await _repo.GetByIdAsync(id);
            if (entity == null) return false;
            entity.ReleasedDate = DateTime.UtcNow;
            entity.Status = 3; // Disposed
            return await _repo.UpdateAsync(entity);
        }

        private static QuarantineActionDTO Map(QuarantaineAction q) => new()
        {
            QuarantaineActionId = q.QuarantaineActionId,
            InventoryLotId = q.InventoryLotId,
            BatchNumber = q.InventoryLot?.BatchNumber,
            ItemName = q.InventoryLot?.Item?.Drug?.GenericName,
            QuarantineDate = q.QuarantineDate,
            Reason = q.Reason,
            ReleasedDate = q.ReleasedDate,
            Status = q.Status,
            StatusName = q.StatusNavigation?.Status
                         ?? q.Status switch { 1 => "Active", 2 => "Released", 3 => "Disposed", _ => "Unknown" }
        };
    }
}
