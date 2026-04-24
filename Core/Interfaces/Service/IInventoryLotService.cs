using PharmaStock.Core.DTO.InventoryLot;

namespace PharmaStock.Core.Interfaces.Service
{
    public interface IInventoryLotService
    {
        Task<InventoryLotDTO> CreateAsync(InventoryLotDTO dto);

        Task<InventoryLotDTO?> GetByIdAsync(int id);

        

      
        Task<IEnumerable<InventoryLotDTO>> SearchAsync(
            int? itemId,
            string? batchNumber,
            int? status,
            DateOnly? expiryBefore,
            DateOnly? expiryAfter);

        Task UpdateAsync(int id, InventoryLotDTO dto);

        Task DeleteAsync(int id);
    }
}