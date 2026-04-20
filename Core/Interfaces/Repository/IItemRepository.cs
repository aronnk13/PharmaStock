using pharmaStock.Core.DTO.Item;
using PharmaStock.Core.DTO.Item;
using PharmaStock.Models;
using Task = System.Threading.Tasks.Task;

namespace PharmaStock.Core.Interfaces.Repository
{
    public interface IItemRepository
    {
        Task<GetItemDTO?> GetItemDtoByIdAsync(int itemId);
        Task<List<GetItemDTO>> GetAllAsync();
        Task AddAsync(Item item);
        Task UpdateAsync(Item item);
        Task<Item?> GetByIdAsync(int itemId);
        Task<ItemDeletedResponseDTO> DeleteItem(int itemId);
    }
}
