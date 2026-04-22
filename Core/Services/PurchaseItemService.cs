using System.Text.Json;
using PharmaStock.Core.DTO;
using PharmaStock.Core.DTO.Item;
using PharmaStock.Core.Interfaces;
using PharmaStock.Core.Interfaces.Repository;
using PharmaStock.Core.Interfaces.Service;
using PharmaStock.Models;

namespace PharmaStock.Core.Services
{
    public class PurchaseItemService : IPurchaseItemService
    {
        private readonly IPurchaseItemRepository _pirepo;
        private readonly IAuditLogService _auditLogService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public PurchaseItemService(IPurchaseItemRepository pirepo, IAuditLogService auditLogService, IHttpContextAccessor httpContextAccessor)
        {
            _pirepo = pirepo;
            _auditLogService = auditLogService;
            _httpContextAccessor = httpContextAccessor;
        }

        private int GetCurrentUserId() =>
            int.TryParse(_httpContextAccessor.HttpContext?.User.FindFirst("userId")?.Value, out var id) ? id : 0;

        public async Task<IEnumerable<PurchaseItemResponseDTO>> GetAllPIAsync()
        {
            var items = await _pirepo.GetAllAsync();

            await _auditLogService.CreateLogAsync(new AuditDto
            {
                UserId = GetCurrentUserId(),
                Action = "PURCHASE_ITEM_LIST_VIEWED",
                Resource = "PurchaseItem:list",
                Metadata = null
            });

            return items.Select(pi =>
            {
                var baseAmount = pi.UnitPrice * pi.OrderedQty;
                var taxAmount = baseAmount * (pi.TaxPct / 100);
                return new PurchaseItemResponseDTO
                {
                    PurchaseItemId = pi.PurchaseItemId,
                    PurchaseOrderId = pi.PurchaseOrderId,
                    ItemId = pi.ItemId,
                    OrderedQty = pi.OrderedQty,
                    UnitPrice = pi.UnitPrice,
                    TaxPct = pi.TaxPct,
                    TaxAmount = taxAmount,
                    TotalAmount = baseAmount + taxAmount
                };
            });
        }

        public async Task<PurchaseItemResponseDTO> AddPIAsync(CreatePurchaseItemDTO dto)
        {
            var isItemValid = await _pirepo.IsItemIdValid(dto.ItemId);
            var isPoIdValid = await _pirepo.IsPurchaseOrderIdValid(dto.PurchaseOrderId);

            if (!isItemValid) throw new Exception("Invalid ItemId");
            if (!isPoIdValid) throw new Exception("Invalid PurchaseOrderId");

            var purchaseItem = new PurchaseItem
            {
                PurchaseOrderId = dto.PurchaseOrderId,
                ItemId = dto.ItemId,
                OrderedQty = dto.OrderedQty,
                UnitPrice = dto.UnitPrice,
                TaxPct = dto.TaxPct
            };
            await _pirepo.AddAsync(purchaseItem);

            var baseAmount = dto.UnitPrice * dto.OrderedQty;
            var taxAmount = baseAmount * (dto.TaxPct / 100);
            var response = new PurchaseItemResponseDTO
            {
                PurchaseItemId = purchaseItem.PurchaseItemId,
                PurchaseOrderId = purchaseItem.PurchaseOrderId,
                ItemId = purchaseItem.ItemId,
                OrderedQty = purchaseItem.OrderedQty,
                UnitPrice = purchaseItem.UnitPrice,
                TaxPct = purchaseItem.TaxPct,
                TaxAmount = taxAmount,
                TotalAmount = baseAmount + taxAmount
            };

            await _auditLogService.CreateLogAsync(new AuditDto
            {
                UserId = GetCurrentUserId(),
                Action = "PURCHASE_ITEM_CREATED",
                Resource = $"PurchaseItem:{purchaseItem.PurchaseItemId}",
                Metadata = JsonSerializer.Serialize(response)
            });

            return response;
        }

        public async Task<PurchaseItemResponseDTO> UpdatePIAsync(int id, UpdatePurchaseItemDTO dto)
        {
            var existingPI = await _pirepo.GetByIdAsync(id);
            if (existingPI == null) throw new Exception("PurchaseItem not found");

            var hasGRN = await _pirepo.HasGRNAsync(id);
            if (hasGRN) throw new Exception("POItem cannot be modified after receipt");

            existingPI.OrderedQty = dto.OrderedQty;
            existingPI.UnitPrice = dto.UnitPrice;
            existingPI.TaxPct = dto.TaxPct;
            _pirepo.Update(existingPI);

            var baseAmount = dto.UnitPrice * dto.OrderedQty;
            var taxAmount = baseAmount * (dto.TaxPct / 100);
            var response = new PurchaseItemResponseDTO
            {
                PurchaseItemId = existingPI.PurchaseItemId,
                PurchaseOrderId = existingPI.PurchaseOrderId,
                ItemId = existingPI.ItemId,
                OrderedQty = existingPI.OrderedQty,
                UnitPrice = existingPI.UnitPrice,
                TaxPct = existingPI.TaxPct,
                TaxAmount = taxAmount,
                TotalAmount = baseAmount + taxAmount
            };

            await _auditLogService.CreateLogAsync(new AuditDto
            {
                UserId = GetCurrentUserId(),
                Action = "PURCHASE_ITEM_UPDATED",
                Resource = $"PurchaseItem:{id}",
                Metadata = JsonSerializer.Serialize(response)
            });

            return response;
        }

        public async System.Threading.Tasks.Task DeletePIAsync(int id)
        {
            var existingPI = await _pirepo.GetByIdAsync(id);
            if (existingPI == null) throw new Exception("PurchaseItem not found");

            var hasGRN = await _pirepo.HasGRNAsync(id);
            if (hasGRN) throw new Exception("POItem cannot be deleted after receipt");

            await _pirepo.DeleteAsync(id);

            await _auditLogService.CreateLogAsync(new AuditDto
            {
                UserId = GetCurrentUserId(),
                Action = "PURCHASE_ITEM_DELETED",
                Resource = $"PurchaseItem:{id}",
                Metadata = null
            });
        }
    }
}
