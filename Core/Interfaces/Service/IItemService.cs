using pharmaStock.Core.DTO.Item;
using PharmaStock.Core.DTO.Item;
using Task = System.Threading.Tasks.Task;

namespace PharmaStock.Core.Interfaces.Service
{
    public interface IItemService
    {
        Task<GetItemDTO> CreateAsync(ItemDTO itemDTO);
        Task<GetItemDTO?> GetByIdAsync(int itemId);
        Task<List<GetItemDTO>> GetAllAsync();
        Task UpdateAsync(int itemId, ItemDTO itemDTO);
        Task<ItemDeletedResponseDTO> DeleteAsync(int itemId);
        Task<bool> ToggleStatusAsync(int itemId);
    }
}
