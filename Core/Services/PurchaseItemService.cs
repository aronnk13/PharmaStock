using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PharmaStock.Core.DTO;
using PharmaStock.Core.DTO.Item;
using PharmaStock.Core.Interfaces;
using PharmaStock.Core.Interfaces.Repository;
using PharmaStock.Core.Interfaces.Service;
using PharmaStock.Infrastructure.Repositories;
using PharmaStock.Models;

namespace PharmaStock.Core.Services
{
    public class PurchaseItemService : IPurchaseItemService
    {
        private readonly IPurchaseItemRepository pirepo;
        private readonly IAuditLogService auditLogService;
        private readonly IHttpContextAccessor httpContextAccessor;
        public PurchaseItemService(IPurchaseItemRepository _pirepo, IAuditLogService _auditLogService, IHttpContextAccessor _httpContextAccessor)
        {
            pirepo = _pirepo;
            auditLogService = _auditLogService;
            httpContextAccessor = _httpContextAccessor;
        }
        public async Task<PurchaseItemResponseDTO> AddPIAsync(CreatePurchaseItemDTO dto)
        {
            var isItemvalid = await pirepo.IsItemIdValid(dto.ItemId);
            var ispoidvalid = await pirepo.IsPurchaseOrderIdValid(dto.PurchaseOrderId);
            if (isItemvalid && ispoidvalid)
            {
                var purchaseItem = new PurchaseItem
                {
                    PurchaseOrderId = dto.PurchaseOrderId,
                    ItemId = dto.ItemId,
                    OrderedQty = dto.OrderedQty,
                    UnitPrice = dto.UnitPrice,
                    TaxPct = dto.TaxPct
                };
                await pirepo.AddAsync(purchaseItem);
                var baseAmount = dto.UnitPrice * dto.OrderedQty;
                var taxAmount = baseAmount * (dto.TaxPct / 100);
                var totalAmount = baseAmount + taxAmount;
                var response = new PurchaseItemResponseDTO
                {
                    PurchaseItemId = purchaseItem.PurchaseItemId,
                    PurchaseOrderId = purchaseItem.PurchaseOrderId,
                    ItemId = purchaseItem.ItemId,
                    OrderedQty = purchaseItem.OrderedQty,
                    UnitPrice = purchaseItem.UnitPrice,
                    TaxPct = purchaseItem.TaxPct,
                    TaxAmount = taxAmount,
                    TotalAmount = totalAmount
                };
                return response;
            }
            else if (!isItemvalid)
            {
                throw new Exception("Invalid ItemId");
            }
            else
            {
                throw new Exception("Invalid PurchaseOrderId");
            }
        }

        public async System.Threading.Tasks.Task DeletePIAsync(int id)
        {
            var existingPI = await pirepo.GetByIdAsync(id);
            if (existingPI == null)
            {
                throw new Exception("PurchaseItem not found");
            }
            var hasGRN = await pirepo.HasGRNAsync(id);
            if (hasGRN)
            {
                throw new Exception("POItem cannot be deleted after receipt");
            }
            await pirepo.DeleteAsync(id);
            var userIdClaim = httpContextAccessor.HttpContext?.User.FindFirst("userId")?.Value;
            int userId = userIdClaim != null ? int.Parse(userIdClaim) : 0;

            await auditLogService.CreateLogAsync(new AuditDto
            {
                UserId = userId,
                Action = "DELETED",
                Resource = "PurchaseItem",
                Metadata = $"PurchaseItemId: {id}"
            });
        }

        public async Task<PurchaseItemResponseDTO> UpdatePIAsync(int id, UpdatePurchaseItemDTO dto)
        {

            var existingPI = await pirepo.GetByIdAsync(id);
            if (existingPI == null)
            {
                throw new Exception("PurchaseItem not found");
            }
            var hasGRN = await pirepo.HasGRNAsync(id);
            if (hasGRN)
            {
                throw new Exception("POItem cannot be modified after receipt");
            }
            existingPI.OrderedQty = dto.OrderedQty;
            existingPI.UnitPrice = dto.UnitPrice;
            existingPI.TaxPct = dto.TaxPct;
            pirepo.Update(existingPI);
            var baseAmount = dto.UnitPrice * dto.OrderedQty;
            var taxAmount = baseAmount * (dto.TaxPct / 100);
            var totalAmount = baseAmount + taxAmount;
            var response = new PurchaseItemResponseDTO
            {
                PurchaseItemId = existingPI.PurchaseItemId,
                PurchaseOrderId = existingPI.PurchaseOrderId,
                ItemId = existingPI.ItemId,
                OrderedQty = existingPI.OrderedQty,
                UnitPrice = existingPI.UnitPrice,
                TaxPct = existingPI.TaxPct,
                TaxAmount = taxAmount,
                TotalAmount = totalAmount
            };
            return response;
        }
    }
}