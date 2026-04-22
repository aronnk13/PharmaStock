using System.Security.Claims;
using System.Text.Json;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PharmaStock.Core.DTO;
using PharmaStock.Core.DTO.PurchaseOrder;
using PharmaStock.Core.Interfaces.Service;

namespace PharmaStock.Controllers.PurchaseOrder
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin,ProcurementOfficer")]
    public class PurchaseOrderController : ControllerBase
    {
        private readonly IPurchaseOrderService _service;
        private readonly IAuditLogService _auditLogService;

        public PurchaseOrderController(IPurchaseOrderService service, IAuditLogService auditLogService)
        {
            _service = service;
            _auditLogService = auditLogService;
        }

        private int GetCurrentUserId()
        {
            var claim = User.FindFirst("userId")?.Value;
            return int.TryParse(claim, out var id) ? id : 0;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _service.GetAllAsync();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _service.GetByIdAsync(id);
            if (result == null)
                return NotFound(new { message = "Purchase order not found." });
            return Ok(result);
        }

        [HttpGet("statuses")]
        public async Task<IActionResult> GetStatuses()
        {
            var result = await _service.GetStatusesAsync();
            return Ok(result);
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] CreatePurchaseOrderDTO dto)
        {
            try
            {
                var result = await _service.CreateAsync(dto);

                await _auditLogService.CreateLogAsync(new AuditDto
                {
                    UserId = GetCurrentUserId(),
                    Action = "PURCHASE_ORDER_CREATED",
                    Resource = $"PurchaseOrder:{result.PurchaseOrderId}",
                    Metadata = JsonSerializer.Serialize(result)
                });

                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdatePurchaseOrderDTO dto)
        {
            try
            {
                var result = await _service.UpdateAsync(id, dto);

                await _auditLogService.CreateLogAsync(new AuditDto
                {
                    UserId = GetCurrentUserId(),
                    Action = "PURCHASE_ORDER_UPDATED",
                    Resource = $"PurchaseOrder:{id}",
                    Metadata = JsonSerializer.Serialize(new { old = dto, @new = result })
                });

                return Ok(result);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _service.DeleteAsync(id);

                await _auditLogService.CreateLogAsync(new AuditDto
                {
                    UserId = GetCurrentUserId(),
                    Action = "PURCHASE_ORDER_DELETED",
                    Resource = $"PurchaseOrder:{id}",
                    Metadata = JsonSerializer.Serialize(new { purchaseOrderId = id })
                });

                return Ok(new { message = "Purchase order deleted successfully." });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
