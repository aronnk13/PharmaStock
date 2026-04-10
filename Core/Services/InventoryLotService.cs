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
            var entity = new InventoryLot
            {
                ItemId = dto.ItemId,
                BatchNumber = dto.BatchNumber,
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

        public async Task<IEnumerable<InventoryLotDTO>> GetAllAsync()
        {
            var lots = await _repository.GetAllAsync();
            return lots.Select(Map);
        }

        public async System.Threading.Tasks.Task UpdateAsync(int id, InventoryLotDTO dto)
        {
            var lot = await _repository.GetByIdAsync(id);
            if (lot == null)
                throw new KeyNotFoundException("Inventory Lot not found");

            lot.ItemId = dto.ItemId;
            lot.BatchNumber = dto.BatchNumber;
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
            BatchNumber = lot.BatchNumber,
            ExpiryDate = lot.ExpiryDate,
            ManufacturerId = lot.ManufacturerId,
            Status = lot.Status
        };
    }
}
