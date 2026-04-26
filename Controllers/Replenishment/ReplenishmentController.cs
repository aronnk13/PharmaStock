using System.Security.Claims;
using System.Text.Json;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PharmaStock.Core.DTO;
using PharmaStock.Core.DTO.Replenishment;
using PharmaStock.Core.Interfaces.Service;
using PharmaStock.Models;

namespace PharmaStock.Controllers.Replenishment
{
    [ApiController]
    [Authorize(Roles = "InventoryController,Admin")]
    [Route("api/replenishment")]
    public class ReplenishmentController : ControllerBase
    {
        private readonly IReplenishmentService _service;
        private readonly PharmaStockContext _context;
        private readonly IAuditLogService _auditLogService;

        public ReplenishmentController(
            IReplenishmentService service,
            PharmaStockContext context,
            IAuditLogService auditLogService)
        {
            _service = service;
            _context = context;
            _auditLogService = auditLogService;
        }

        private int GetCurrentUserId()
        {
            var claim = User.FindFirst("userId")?.Value;
            if (!int.TryParse(claim, out var id))
                throw new InvalidOperationException("userId claim is missing or invalid in the JWT token.");
            return id;
        }

        // ── Lookup ──────────────────────────────────────────────────────────────

        [HttpGet("items-lookup")]
        public async Task<IActionResult> GetItemsLookup()
        {
            var items = await (
                from i in _context.Items
                join d in _context.Drugs on i.DrugId equals d.DrugId into drugJoin
                from drug in drugJoin.DefaultIfEmpty()
                select new
                {
                    i.ItemId,
                    Name = drug != null
                        ? drug.GenericName + (drug.BrandName != null ? " (" + drug.BrandName + ")" : "")
                        : "Item #" + i.ItemId,
                    Strength = drug != null ? drug.Strength : ""
                }
            ).ToListAsync();
            return Ok(items);
        }

        // ── Requests ────────────────────────────────────────────────────────────

        [HttpGet("requests")]
        public async Task<IActionResult> GetAllRequests()
        {
            var result = await _service.GetAllRequestsAsync();
            return Ok(result);
        }

        [HttpGet("requests/{id}")]
        public async Task<IActionResult> GetRequestById(int id)
        {
            var result = await _service.GetRequestByIdAsync(id);
            return result == null ? NotFound() : Ok(result);
        }

        [HttpGet("requests/status/{status}")]
        public async Task<IActionResult> GetByStatus(int status)
        {
            var result = await _service.GetRequestsByStatusAsync(status);
            return Ok(result);
        }

        [HttpPost("requests")]
        public async Task<IActionResult> CreateRequest([FromBody] CreateReplenishmentRequestDTO dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var result = await _service.CreateRequestAsync(dto);

            await _auditLogService.CreateLogAsync(new AuditDto
            {
                UserId = GetCurrentUserId(),
                Action = "REPLENISHMENT_REQUEST_CREATED",
                Resource = $"ReplenishmentRequest:{result.ReplenishmentRequestId}",
                Metadata = JsonSerializer.Serialize(result)
            });

            return Ok(result);
        }

        [HttpPatch("requests/{id}/status")]
        public async Task<IActionResult> UpdateStatus(int id, [FromQuery] int status)
        {
            var success = await _service.UpdateRequestStatusAsync(id, status);
            if (!success) return NotFound();

            await _auditLogService.CreateLogAsync(new AuditDto
            {
                UserId = GetCurrentUserId(),
                Action = "REPLENISHMENT_REQUEST_STATUS_UPDATED",
                Resource = $"ReplenishmentRequest:{id}",
                Metadata = JsonSerializer.Serialize(new { replenishmentRequestId = id, status })
            });

            return Ok(new { success = true });
        }

        // ── Rules ────────────────────────────────────────────────────────────────

        [HttpGet("rules")]
        public async Task<IActionResult> GetAllRules()
        {
            var result = await _service.GetAllRulesAsync();
            return Ok(result);
        }

        [HttpPost("rules")]
        public async Task<IActionResult> CreateRule([FromBody] CreateReplenishmentRuleDTO dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var result = await _service.CreateRuleAsync(dto);

            await _auditLogService.CreateLogAsync(new AuditDto
            {
                UserId = GetCurrentUserId(),
                Action = "REPLENISHMENT_RULE_CREATED",
                Resource = $"ReplenishmentRule:{result.ReplenishmentRuleId}",
                Metadata = JsonSerializer.Serialize(result)
            });

            return Ok(result);
        }

        [HttpPut("rules/{id}")]
        public async Task<IActionResult> UpdateRule(int id, [FromBody] CreateReplenishmentRuleDTO dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var success = await _service.UpdateRuleAsync(id, dto);
            if (!success) return NotFound();

            await _auditLogService.CreateLogAsync(new AuditDto
            {
                UserId = GetCurrentUserId(),
                Action = "REPLENISHMENT_RULE_UPDATED",
                Resource = $"ReplenishmentRule:{id}",
                Metadata = JsonSerializer.Serialize(new { ruleId = id })
            });

            return Ok(new { success = true });
        }

        [HttpDelete("rules/{id}")]
        public async Task<IActionResult> DeleteRule(int id)
        {
            var success = await _service.DeleteRuleAsync(id);
            if (!success) return NotFound();

            await _auditLogService.CreateLogAsync(new AuditDto
            {
                UserId = GetCurrentUserId(),
                Action = "REPLENISHMENT_RULE_DELETED",
                Resource = $"ReplenishmentRule:{id}",
                Metadata = JsonSerializer.Serialize(new { ruleId = id })
            });

            return Ok(new { success = true });
        }

        // ── Auto Replenishment ───────────────────────────────────────────────────

        [HttpPost("run-check")]
        public async Task<IActionResult> RunCheck()
        {
            try
            {
                var result = await _service.RunReplenishmentCheckAsync();
                try { await _auditLogService.CreateLogAsync(new AuditDto { UserId = GetCurrentUserId(), Action = "REPLENISHMENT_CHECK_RUN", Resource = "ReplenishmentCheck", Metadata = JsonSerializer.Serialize(result) }); } catch { }
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpPost("{reqId}/convert")]
        public async Task<IActionResult> ConvertToTransferOrder(int reqId)
        {
            var result = await _service.ConvertToTransferOrderAsync(reqId);
            if (result == null) return NotFound(new { message = "Request not found or not in Open status." });

            await _auditLogService.CreateLogAsync(new AuditDto
            {
                UserId = GetCurrentUserId(),
                Action = "REPLENISHMENT_CONVERTED_TO_TRANSFER",
                Resource = $"TransferOrder:{result.TransferOrderId}",
                Metadata = JsonSerializer.Serialize(new { reqId, transferOrderId = result.TransferOrderId })
            });

            return Ok(result);
        }
    }
}
