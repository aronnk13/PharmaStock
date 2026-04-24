using PharmaStock.Core.DTO.InventoryLot;
using PharmaStock.Core.Interfaces.Repository;
using PharmaStock.Core.Interfaces.Service;
using PharmaStock.Models;

namespace PharmaStock.Core.Services
{
    public class InventoryLotService : IInventoryLotService
    {
        private readonly IInventoryLotRepository _repository;

        public InventoryLotService(IInventoryLotRepository repository)
        {
            _repository = repository;
        }

        public async Task<InventoryLotDTO> CreateAsync(InventoryLotDTO dto)
        {
            if (!int.TryParse(dto.BatchNumber?.Trim(), out var batchInt))
                throw new ArgumentException("BatchNumber must be numeric.");

            var entity = new InventoryLot
            {
                ItemId = dto.ItemId,
                BatchNumber = batchInt,
                ExpiryDate = dto.ExpiryDate,
                ManufacturerId = dto.ManufacturerId,
                Status = dto.Status
            };

            await _repository.AddAsync(entity);

            dto.InventoryLotId = entity.InventoryLotId;
            return dto;
        }

        public async Task<InventoryLotDTO?> GetByIdAsync(int id)
        {
            var lot = await _repository.GetByIdAsync(id);
            return lot == null ? null : Map(lot);
        }

        

        public async Task<IEnumerable<InventoryLotDTO>> SearchAsync(
        int? itemId,
        string? batchNumber,
        int? status,
        DateOnly? expiryBefore,
        DateOnly? expiryAfter)
        {
            var lots = await _repository.GetAllAsync();

            var query = lots.AsQueryable();

            if (itemId.HasValue)
                query = query.Where(l => l.ItemId == itemId.Value);

            if (!string.IsNullOrEmpty(batchNumber))
                query = query.Where(l => l.BatchNumber.ToString() == batchNumber);

            if (status.HasValue)
                query = query.Where(l => l.Status == status.Value);

            if (expiryBefore.HasValue)
                query = query.Where(l => l.ExpiryDate < expiryBefore.Value);

            if (expiryAfter.HasValue)
                query = query.Where(l => l.ExpiryDate > expiryAfter.Value);

            return query.Select(Map);
        }


        public async System.Threading.Tasks.Task UpdateAsync(int id, InventoryLotDTO dto)
        {
            var lot = await _repository.GetByIdAsync(id);
            if (lot == null)
                throw new KeyNotFoundException("Inventory Lot not found");

            if (!int.TryParse(dto.BatchNumber?.Trim(), out var updBatchInt))
                throw new ArgumentException("BatchNumber must be numeric.");

            lot.ItemId = dto.ItemId;
            lot.BatchNumber = updBatchInt;
            lot.ExpiryDate = dto.ExpiryDate;
            lot.ManufacturerId = dto.ManufacturerId;
            lot.Status = dto.Status;

            await _repository.UpdateAsync(lot);
        }

        public async System.Threading.Tasks.Task DeleteAsync(int id)
        {
            await _repository.DeleteAsync(id);
        }

        private static InventoryLotDTO Map(InventoryLot lot) => new()
        {
            InventoryLotId = lot.InventoryLotId,
            ItemId = lot.ItemId,
            BatchNumber = lot.BatchNumber.ToString(),   // int entity → string DTO
            ExpiryDate = lot.ExpiryDate,
            ManufacturerId = lot.ManufacturerId,
            Status = lot.Status
        };
    }
}
