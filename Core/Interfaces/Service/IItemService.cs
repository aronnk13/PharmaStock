using PharmaStock.Core.DTO.Item;

namespace PharmaStock.Core.Interfaces.Service
{
    public interface IItemService
    {
        Task<int> CreateItemAsync(CreateItemDTO dto);
        Task UpdateItemAsync(UpdateItemDTO dto);
    }
}