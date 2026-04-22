using System.Security.Claims;
using System.Text.Json;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PharmaStock.Core.DTO;
using PharmaStock.Core.DTO.InventoryLot;
using PharmaStock.Core.Interfaces.Service;

namespace PharmaStock.Controllers.InventoryLot
{
    [ApiController]
    [Authorize(Roles = "InventoryController")]
    [Route("api/inventorylot")]
    public class InventoryLotController : ControllerBase
    {
        private readonly IInventoryLotService _service;
        private readonly IAuditLogService _auditLogService;

        public InventoryLotController(IInventoryLotService service, IAuditLogService auditLogService)
        {
            _service = service;
            _auditLogService = auditLogService;
        }

        private int GetCurrentUserId()
        {
            var claim = User.FindFirst("userId")?.Value;
            return int.TryParse(claim, out var id) ? id : 0;
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create(InventoryLotDTO dto)
        {
            var result = await _service.CreateAsync(dto);

            await _auditLogService.CreateLogAsync(new AuditDto
            {
                UserId = GetCurrentUserId(),
                Action = "INVENTORY_LOT_CREATED",
                Resource = $"InventoryLot:{result.InventoryLotId}",
                Metadata = JsonSerializer.Serialize(result)
            });

            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var lot = await _service.GetByIdAsync(id);
            return lot == null ? NotFound() : Ok(lot);
        }


        [HttpGet("search")]
        public async Task<IActionResult> Search(
        [FromQuery] int? itemId,
        [FromQuery] int? batchNumber,
        [FromQuery] int? status,
        [FromQuery] DateOnly? expiryBefore,
        [FromQuery] DateOnly? expiryAfter)
        {
            var result = await _service.SearchAsync(
                itemId,
                batchNumber,
                status,
                expiryBefore,
                expiryAfter);

            return Ok(result);
        }


        [HttpPut("update/{id}")]
        public async Task<IActionResult> Update(int id, InventoryLotDTO dto)
        {
            await _service.UpdateAsync(id, dto);

            await _auditLogService.CreateLogAsync(new AuditDto
            {
                UserId = GetCurrentUserId(),
                Action = "INVENTORY_LOT_UPDATED",
                Resource = $"InventoryLot:{id}",
                Metadata = JsonSerializer.Serialize(dto)
            });

            return Ok(new { success = true });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.DeleteAsync(id);

            await _auditLogService.CreateLogAsync(new AuditDto
            {
                UserId = GetCurrentUserId(),
                Action = "INVENTORY_LOT_DELETED",
                Resource = $"InventoryLot:{id}",
                Metadata = JsonSerializer.Serialize(new { inventoryLotId = id })
            });

            return Ok(new { success = true });
        }
    }
}