using PharmaStock.Core.DTO.Item;
using PharmaStock.Core.Interfaces.Repository;
using PharmaStock.Core.Interfaces.Service;
using PharmaStock.Models;

namespace PharmaStock.Core.Services
{
    public class ItemService : IItemService
    {
        private readonly IItemRepository _itemRepository;

        public ItemService(IItemRepository itemRepository)
        {
            _itemRepository = itemRepository;
        }


        public async Task<int> CreateAsync(ItemDTO itemDTO)
        {
            var item = new Item
            {
                DrugId = itemDTO.DrugId,
                PackSize = itemDTO.PackSize,
                UoM = itemDTO.UoM,
                ConversionToEach = itemDTO.ConversionToEach,
                Barcode = itemDTO.Barcode,
                Status = itemDTO.Status
            };

            await _itemRepository.AddAsync(item);
            return item.ItemId;
        }


        public async System.Threading.Tasks.Task UpdateAsync(int itemId, ItemDTO itemDTO)
        {
            var item = await _itemRepository.GetByIdAsync(itemId);

            if (item == null)
                throw new KeyNotFoundException("Item not found");

            item.DrugId = itemDTO.DrugId;
            item.PackSize = itemDTO.PackSize;
            item.UoM = itemDTO.UoM;
            item.ConversionToEach = itemDTO.ConversionToEach;
            item.Barcode = itemDTO.Barcode;
            item.Status = itemDTO.Status;

            await _itemRepository.UpdateAsync(item);
        }



        public async Task<ItemDTO?> GetByIdAsync(int itemId)
        {
            var item = await _itemRepository.GetByIdAsync(itemId);

            if (item == null)
                return null;

            return new ItemDTO
            {
                ItemId = item.ItemId,
                DrugId = item.DrugId,
                PackSize = item.PackSize,
                UoM = item.UoM,
                ConversionToEach = item.ConversionToEach,
                Barcode = item.Barcode!,
                Status = item.Status
            };
        }
        public async Task<List<ItemDTO>> GetItemsAsync(int? drugId = null)
        {
            var items = await _itemRepository.GetItemsAsync(drugId);

            return items.Select(i => new ItemDTO
            {
                ItemId = i.ItemId,
                DrugId = i.DrugId,
                PackSize = i.PackSize,
                UoM = i.UoM,
                ConversionToEach = i.ConversionToEach,
                Barcode = i.Barcode!,
                Status = i.Status
            }).ToList();
        }
    }
}
