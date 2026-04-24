using PharmaStock.Core.DTO.Pharmacist;
using PharmaStock.Core.Interfaces.Repository;
using PharmaStock.Core.Interfaces.Service;
using PharmaStock.Models;

namespace PharmaStock.Core.Services
{
    public class DispenseService : IDispenseService
    {
        private readonly IDispenseRepository _repo;

        public DispenseService(IDispenseRepository repo) => _repo = repo;

        public async Task<IEnumerable<DispenseRefDTO>> GetAllAsync()
        {
            var items = await _repo.GetAllWithDetailsAsync();
            return items.Select(Map);
        }

        public async Task<IEnumerable<DispenseRefDTO>> GetByLocationAsync(int locationId)
        {
            var items = await _repo.GetByLocationAsync(locationId);
            return items.Select(Map);
        }

        public async Task<DispenseRefDTO?> GetByIdAsync(int id)
        {
            var item = await _repo.GetByIdWithDetailsAsync(id);
            return item == null ? null : Map(item);
        }

        public async Task<DispenseRefDTO> CreateAsync(CreateDispenseRefDTO dto)
        {
            var entity = new DispenseRef
            {
                LocationId = dto.LocationId,
                ItemId = dto.ItemId,
                InventoryLotId = dto.InventoryLotId,
                Quantity = dto.Quantity,
                Destination = dto.Destination,
                DispenseDate = DateTime.UtcNow,
                Status = false
            };
            await _repo.AddAsync(entity);
            var created = await _repo.GetByIdWithDetailsAsync(entity.DispenseRefId);
            return Map(created ?? entity);
        }

        private static DispenseRefDTO Map(DispenseRef d) => new()
        {
            DispenseRefId = d.DispenseRefId,
            LocationId = d.LocationId,
            LocationName = d.Location?.Name,
            ItemId = d.ItemId,
            ItemName = d.Item?.Drug?.GenericName,
            InventoryLotId = d.InventoryLotId,
            BatchNumber = d.InventoryLot?.BatchNumber.ToString(),
            Quantity = d.Quantity,
            DispenseDate = d.DispenseDate,
            Status = d.Status,
            Destination = d.Destination,
            DestinationName = d.DestinationNavigation?.Type
        };
    }
}
