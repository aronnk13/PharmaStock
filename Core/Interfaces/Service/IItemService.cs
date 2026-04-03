using PharmaStock.Core.DTO.Item;

namespace PharmaStock.Core.Interfaces.Service
{
    public interface IItemService
    {
        Task<int> CreateAsync(CreateItemDTO dto);
        Task UpdateAsync(UpdateItemDTO dto);

       
        Task<ItemResponseDTO?> GetByIdAsync(int itemId);
    }
}
