using System.Security.Claims;
using System.Text.Json;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PharmaStock.Core.DTO;
using PharmaStock.Core.DTO.GRNItem;
using PharmaStock.Core.Interfaces.Service;

namespace PharmaStock.Controllers.GRNItem
{
    [ApiController]
    [Route("api/v1/goods-receipt")]
    [Authorize(Roles = "Admin,Pharmacist,InventoryController")]
    public class GRNItemController : ControllerBase
    {
        private readonly IGRNItemService _service;
        private readonly IAuditLogService _auditLogService;

        public GRNItemController(IGRNItemService service, IAuditLogService auditLogService)
        {
            _service = service;
            _auditLogService = auditLogService;
        }

        private int GetCurrentUserId()
        {
            var claim = User.FindFirst("userId")?.Value;
            return int.TryParse(claim, out var id) ? id : 0;
        }

        [HttpPost("CreateGRNItem")]
        public async Task<IActionResult> Create([FromBody] CreateGRNItemDTO dto)
        {
            try
            {
                var result = await _service.CreateAsync(dto);

                await _auditLogService.CreateLogAsync(new AuditDto
                {
                    UserId = GetCurrentUserId(),
                    Action = "GRN_ITEM_CREATED",
                    Resource = $"GoodsReceipt:{dto.GoodsReceiptId}",
                    Metadata = JsonSerializer.Serialize(result)
                });

                return StatusCode(201, result);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpGet("GetGRNItems")]
        public async Task<IActionResult> Get([FromQuery] GRNItemFilterDTO? filter)
        {
            try
            {
                var result = await _service.GetAsync(filter ?? new GRNItemFilterDTO());
                return Ok(result);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpPut("UpdateGRNItem")]
        [Authorize(Roles = "Admin,Quality Officer")]
        public async Task<IActionResult> Update([FromBody] UpdateGRNItemDTO dto)
        {
            try
            {
                await _service.UpdateAsync(dto);

                await _auditLogService.CreateLogAsync(new AuditDto
                {
                    UserId = GetCurrentUserId(),
                    Action = "GRN_ITEM_UPDATED",
                    Resource = $"GRNItem:{dto.GoodsReceiptItemId}",
                    Metadata = JsonSerializer.Serialize(dto)
                });

                return Ok(new { message = "GRNItem updated successfully." });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }
    }
}
