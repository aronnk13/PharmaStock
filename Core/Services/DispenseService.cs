using Microsoft.EntityFrameworkCore;
using PharmaStock.Core.DTO.Pharmacist;
using PharmaStock.Core.Interfaces.Repository;
using PharmaStock.Core.Interfaces.Service;
using PharmaStock.Models;

namespace PharmaStock.Core.Services
{
    public class DispenseService : IDispenseService
    {
        private readonly IDispenseRepository _repo;
        private readonly PharmaStockContext _context;

        public DispenseService(IDispenseRepository repo, PharmaStockContext context)
        {
            _repo = repo;
            _context = context;
        }

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
            // Check and deduct from InventoryBalance
            var balance = await _context.InventoryBalances
                .FirstOrDefaultAsync(b => b.LocationId == dto.LocationId
                                       && b.ItemId == dto.ItemId
                                       && b.InventoryLotId == dto.InventoryLotId);

            if (balance == null)
                throw new Exception("No stock found at this location for the selected item/lot.");

            if (balance.QuantityOnHand < dto.Quantity)
                throw new Exception($"Insufficient stock. Available: {balance.QuantityOnHand}, Requested: {dto.Quantity}.");

            balance.QuantityOnHand -= dto.Quantity;

            var entity = new DispenseRef
            {
                LocationId = dto.LocationId,
                ItemId = dto.ItemId,
                InventoryLotId = dto.InventoryLotId,
                Quantity = dto.Quantity,
                Destination = dto.Destination,
                DispenseDate = DateTime.UtcNow,
                Status = true
            };

            await _repo.AddAsync(entity);
            await _context.SaveChangesAsync();

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
