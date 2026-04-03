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

        // ✅ CREATE
        public async Task<int> CreateAsync(CreateItemDTO dto)
        {
            var item = new Item
            {
                DrugId = dto.DrugId,
                PackSize = dto.PackSize,
                UoM = dto.UoM,
                ConversionToEach = dto.ConversionToEach,
                Barcode = dto.Barcode,
                Status = dto.Status
            };

            await _itemRepository.AddAsync(item);
            return item.ItemId;
        }

        // ✅ UPDATE
        public async System.Threading.Tasks.Task UpdateAsync(UpdateItemDTO dto)
        {
            var item = await _itemRepository.GetByIdAsync(dto.ItemId);

            if (item == null)
                throw new KeyNotFoundException("Item not found");

            item.DrugId = dto.DrugId;
            item.PackSize = dto.PackSize;
            item.UoM = dto.UoM;
            item.ConversionToEach = dto.ConversionToEach;
            item.Barcode = dto.Barcode;
            item.Status = dto.Status;

            await _itemRepository.UpdateAsync(item);
        }

        // ✅ GET BY ID (FOR EDIT FLOW)
        public async Task<ItemResponseDTO?> GetByIdAsync(int itemId)
        {
            var item = await _itemRepository.GetByIdAsync(itemId);

            if (item == null)
                return null;

            return new ItemResponseDTO
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
    }
}
