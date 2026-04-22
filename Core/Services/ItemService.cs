using System.Text.Json;
using pharmaStock.Core.DTO.Item;
using PharmaStock.Core.DTO;
using PharmaStock.Core.DTO.Item;
using PharmaStock.Core.Interfaces;
using PharmaStock.Core.Interfaces.Repository;
using PharmaStock.Core.Interfaces.Service;
using PharmaStock.Models;
using Task = System.Threading.Tasks.Task;

namespace PharmaStock.Core.Services
{
    public class ItemService : IItemService
    {
        private readonly IItemRepository _itemRepository;
        private readonly IAuditLogService _auditLogService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ItemService(IItemRepository itemRepository, IAuditLogService auditLogService, IHttpContextAccessor httpContextAccessor)
        {
            _itemRepository = itemRepository;
            _auditLogService = auditLogService;
            _httpContextAccessor = httpContextAccessor;
        }

        private int GetCurrentUserId() =>
            int.TryParse(_httpContextAccessor.HttpContext?.User.FindFirst("userId")?.Value, out var id) ? id : 0;

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
            var result = await _itemRepository.GetItemDtoByIdAsync(item.ItemId)!;

            await _auditLogService.CreateLogAsync(new AuditDto
            {
                UserId = GetCurrentUserId(),
                Action = "ITEM_CREATED",
                Resource = $"Item:{item.ItemId}",
                Metadata = JsonSerializer.Serialize(result)
            });

            return result;
        }

        public async Task<GetItemDTO?> GetByIdAsync(int itemId)
        {
            var result = await _itemRepository.GetItemDtoByIdAsync(itemId);
            if (result == null) return null;

            await _auditLogService.CreateLogAsync(new AuditDto
            {
                UserId = GetCurrentUserId(),
                Action = "ITEM_VIEWED",
                Resource = $"Item:{itemId}",
                Metadata = null
            });

            return result;
        }

        public async Task<List<GetItemDTO>> GetAllAsync()
        {
            var result = await _itemRepository.GetAllAsync();

            await _auditLogService.CreateLogAsync(new AuditDto
            {
                UserId = GetCurrentUserId(),
                Action = "ITEM_LIST_VIEWED",
                Resource = "Item:list",
                Metadata = null
            });

            return result;
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

            await _auditLogService.CreateLogAsync(new AuditDto
            {
                UserId = GetCurrentUserId(),
                Action = "ITEM_UPDATED",
                Resource = $"Item:{itemId}",
                Metadata = JsonSerializer.Serialize(request)
            });
        }

        public async Task<ItemDeletedResponseDTO> DeleteAsync(int itemId)
        {
            var response = await _itemRepository.DeleteItem(itemId);

            if (response.IsDeleted)
            {
                await _auditLogService.CreateLogAsync(new AuditDto
                {
                    UserId = GetCurrentUserId(),
                    Action = "ITEM_DELETED",
                    Resource = $"Item:{itemId}",
                    Metadata = null
                });
            }

            return response;
        }
    }
}
