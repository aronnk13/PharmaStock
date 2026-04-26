using Microsoft.EntityFrameworkCore;
using PharmaStock.Core.DTO.InventoryLot;
using PharmaStock.Core.Interfaces.Repository;
using PharmaStock.Core.Interfaces.Service;
using PharmaStock.Models;

namespace PharmaStock.Core.Services
{
    public class InventoryLotService : IInventoryLotService
    {
        private readonly IInventoryLotRepository _repository;
        private readonly PharmaStockContext _context;

        public InventoryLotService(IInventoryLotRepository repository, PharmaStockContext context)
        {
            _repository = repository;
            _context = context;
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

        

        public async Task<IEnumerable<InventoryLotDTO>> SearchAsync(
            int? itemId,
            string? batchNumber,
            int? status,
            DateOnly? expiryBefore,
            DateOnly? expiryAfter)
        {
            var query = _context.InventoryLots
                .Include(l => l.Item).ThenInclude(i => i.Drug)
                .AsQueryable();

            if (itemId.HasValue)
                query = query.Where(l => l.ItemId == itemId.Value);

            if (!string.IsNullOrEmpty(batchNumber))
                query = query.Where(l => l.BatchNumber.Contains(batchNumber));

            if (status.HasValue)
                query = query.Where(l => l.Status == status.Value);

            if (expiryBefore.HasValue)
                query = query.Where(l => l.ExpiryDate < expiryBefore.Value);

            if (expiryAfter.HasValue)
                query = query.Where(l => l.ExpiryDate > expiryAfter.Value);

            return await query.Select(l => new InventoryLotDTO
            {
                InventoryLotId = l.InventoryLotId,
                ItemId         = l.ItemId,
                ItemName       = l.Item != null ? l.Item.Drug.GenericName : null,
                BatchNumber    = l.BatchNumber,
                ExpiryDate     = l.ExpiryDate,
                ManufacturerId = l.ManufacturerId,
                Status         = l.Status
            }).ToListAsync();
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
            ItemId         = lot.ItemId,
            ItemName       = lot.Item?.Drug?.GenericName,
            BatchNumber    = lot.BatchNumber,
            ExpiryDate     = lot.ExpiryDate,
            ManufacturerId = lot.ManufacturerId,
            Status         = lot.Status
        };
    }
}
