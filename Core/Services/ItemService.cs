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

        public async Task<int> CreateItemAsync(CreateItemDTO dto)
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

        public async System.Threading.Tasks.Task UpdateItemAsync(UpdateItemDTO dto)
        {
            var item = await _itemRepository.GetByIdAsync(dto.ItemId);

            if (item == null)
                throw new Exception("Item not found");

            item.DrugId = dto.DrugId;
            item.PackSize = dto.PackSize;
            item.UoM = dto.UoM;
            item.ConversionToEach = dto.ConversionToEach;
            item.Barcode = dto.Barcode;
            item.Status = dto.Status;

            await _itemRepository.UpdateAsync(item);
        }
        
    }
}
