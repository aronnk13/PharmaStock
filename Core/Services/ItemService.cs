using pharmaStock.Core.DTO.Item;
using PharmaStock.Core.DTO.Item;
using PharmaStock.Core.Interfaces.Repository;
using PharmaStock.Core.Interfaces.Service;
using PharmaStock.Models;
using Task = System.Threading.Tasks.Task;

namespace PharmaStock.Core.Services
{
    public class ItemService : IItemService
    {
        private readonly IItemRepository _itemRepository;

        public ItemService(IItemRepository itemRepository)
        {
            _itemRepository = itemRepository;
        }

        public async Task<GetItemDTO> CreateAsync(ItemDTO request)
        {
            var item = new Item
            {
                DrugId = request.DrugId,
                PackSize = request.PackSize,
                UoM = request.UoMId,
                ConversionToEach = request.ConversionToEach,
                Barcode = request.Barcode,
                Status = request.Status
            };

            await _itemRepository.AddAsync(item);
            return await _itemRepository.GetItemDtoByIdAsync(item.ItemId)!;
        }

        public async Task<GetItemDTO?> GetByIdAsync(int itemId)
        {
            return await _itemRepository.GetItemDtoByIdAsync(itemId);
        }

        public async Task<List<GetItemDTO>> GetAllAsync()
        {
            return await _itemRepository.GetAllAsync();
        }

        public async Task UpdateAsync(int itemId, ItemDTO request)
        {
            var item = await _itemRepository.GetByIdAsync(itemId);
            if (item == null)
                throw new KeyNotFoundException("ITEM_NOT_FOUND");

            item.DrugId = request.DrugId;
            item.PackSize = request.PackSize;
            item.UoM = request.UoMId;
            item.ConversionToEach = request.ConversionToEach;
            item.Barcode = request.Barcode;
            item.Status = request.Status;

            await _itemRepository.UpdateAsync(item);
        }

        public async Task<ItemDeletedResponseDTO> DeleteAsync(int itemId)
        {
            return await _itemRepository.DeleteItem(itemId);
        }

        public async Task<bool> ToggleStatusAsync(int itemId)
        {
            return await _itemRepository.ToggleStatusAsync(itemId);
        }
    }
}
