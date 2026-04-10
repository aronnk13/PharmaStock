using PharmaStock.Core.DTO.InventoryLot;

namespace PharmaStock.Core.Interfaces.Service
{
    public interface IInventoryLotService
    {
        Task<InventoryLotDTO> CreateAsync(InventoryLotDTO dto);
        Task<InventoryLotDTO?> GetByIdAsync(int id);
        Task<IEnumerable<InventoryLotDTO>> GetAllAsync();
        Task UpdateAsync(int id, InventoryLotDTO dto);
        Task DeleteAsync(int id);
    }
}
