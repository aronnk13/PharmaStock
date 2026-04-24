using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PharmaStock.Core.DTO;
using PharmaStock.Core.DTO.Item;
using PharmaStock.Core.Interfaces.Service;

namespace PharmaStock.Controllers.ProductItem
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class PurchaseItemController : ControllerBase
    {
        private readonly IPurchaseItemService service;
        private readonly IAuditLogService _auditLogService;
        public PurchaseItemController(IPurchaseItemService _service, IAuditLogService auditLogService)
        {
            service = _service;
            _auditLogService = auditLogService;
        }

        private int GetCurrentUserId()
        {
            var claim = User.FindFirst("userId")?.Value;
            return int.TryParse(claim, out var id) ? id : 0;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllPurchaseItems()
        {
            try
            {
                var res = await service.GetAllPIAsync();
                return Ok(res);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> CreatePurchaseItem(CreatePurchaseItemDTO dto)
        {
            try
            {
                var res = await service.AddPIAsync(dto);

                await _auditLogService.CreateLogAsync(new AuditDto
                {
                    UserId = GetCurrentUserId(),
                    Action = "PURCHASE_ITEM_CREATED",
                    Resource = $"PurchaseItem:{res.PurchaseItemId}",
                    Metadata = JsonSerializer.Serialize(res)
                });

                return Ok(res);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> UpdatePurchaseItem(int id, UpdatePurchaseItemDTO dto)
        {
            try
            {
                var res = await service.UpdatePIAsync(id, dto);

                await _auditLogService.CreateLogAsync(new AuditDto
                {
                    UserId = GetCurrentUserId(),
                    Action = "PURCHASE_ITEM_UPDATED",
                    Resource = $"PurchaseItem:{id}",
                    Metadata = JsonSerializer.Serialize(res)
                });

                return Ok(res);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> DeletePurchaseItem(int id)
        {
            try
            {
                await service.DeletePIAsync(id);

                await _auditLogService.CreateLogAsync(new AuditDto
                {
                    UserId = GetCurrentUserId(),
                    Action = "PURCHASE_ITEM_DELETED",
                    Resource = $"PurchaseItem:{id}",
                    Metadata = JsonSerializer.Serialize(new { purchaseItemId = id })
                });

                return Ok("PurchaseItem deleted successfully");
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    message = ex.Message,
                    inner = ex.InnerException?.Message,
                    stack = ex.StackTrace
                });
            }
        }
    }
}