using Microsoft.EntityFrameworkCore;
using PharmaStock.Core.DTO.QCO;
using PharmaStock.Core.Interfaces.Repository;
using PharmaStock.Core.Interfaces.Service;
using PharmaStock.Models;
using PharmaStock.Models;

namespace PharmaStock.Core.Services
{
    public class QuarantineService : IQuarantineService
    {
        private readonly IQuarantineRepository _repo;
        private readonly PharmaStockContext _context;

        public QuarantineService(IQuarantineRepository repo, PharmaStockContext context)
        {
            _repo = repo;
            _context = context;
        }

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

            // Mark the inventory lot as Quarantined (status = 2)
            var lot = await _context.InventoryLots.FindAsync(dto.InventoryLotId);
            if (lot != null)
            {
                lot.Status = 2;
                await _context.SaveChangesAsync();
            }

            var created = await _repo.GetByIdWithDetailsAsync(entity.QuarantaineActionId);
            return Map(created ?? entity);
        }

        public async Task<bool> ReleaseAsync(int id)
        {
            var entity = await _repo.GetByIdAsync(id);
            if (entity == null) return false;
            entity.ReleasedDate = DateTime.UtcNow;
            entity.Status = 2;

            // Restore the inventory lot to Active (status = 1)
            var lot = await _context.InventoryLots.FindAsync(entity.InventoryLotId);
            if (lot != null) lot.Status = 1;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DisposeAsync(int id)
        {
            var entity = await _repo.GetByIdAsync(id);
            if (entity == null) return false;
            entity.ReleasedDate = DateTime.UtcNow;
            entity.Status = 3; // Disposed

            // Mark the inventory lot as Disposed (status = 4) and zero out balances
            var lot = await _context.InventoryLots.FindAsync(entity.InventoryLotId);
            if (lot != null)
            {
                lot.Status = 4;
                var balances = await _context.InventoryBalances
                    .Where(b => b.InventoryLotId == entity.InventoryLotId)
                    .ToListAsync();
                foreach (var b in balances)
                    b.QuantityOnHand = 0;
            }

            await _context.SaveChangesAsync();
            return true;
        }

        private static QuarantineActionDTO Map(QuarantaineAction q) => new()
        {
            QuarantaineActionId = q.QuarantaineActionId,
            InventoryLotId = q.InventoryLotId,
            BatchNumber = q.InventoryLot?.BatchNumber.ToString(),
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
