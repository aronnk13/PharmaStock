using PharmaStock.Core.DTO.Item;
using System.Threading.Tasks;

namespace PharmaStock.Core.Interfaces.Service
{
    public interface IItemService
    {
        Task<int> CreateAsync(ItemDTO itemDTO);

        Task UpdateAsync(int itemId, ItemDTO itemDTO);

        Task<ItemDTO?> GetByIdAsync(int itemId);
        Task<List<ItemDTO>> GetItemsAsync(int? drugId = null);
    }
}
