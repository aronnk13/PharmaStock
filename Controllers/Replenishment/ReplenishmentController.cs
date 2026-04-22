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
    [Authorize(Roles = "InventoryController")]
    [Route("api/replenishment")]
    public class ReplenishmentController : ControllerBase
    {
        private readonly IReplenishmentService _service;
        private readonly PharmaStockContext _context;
        private readonly IAuditLogService _auditLogService;

        public ReplenishmentController(IReplenishmentService service, PharmaStockContext context, IAuditLogService auditLogService)
        {
            _service = service;
            _context = context;
            _auditLogService = auditLogService;
        }

        private int GetCurrentUserId()
        {
            var claim = User.FindFirst("userId")?.Value;
            return int.TryParse(claim, out var id) ? id : 0;
        }

        /// <summary>Returns items with their drug names for use in dropdowns.</summary>
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

        [HttpGet("rules")]
        public async Task<IActionResult> GetAllRules()
        {
            var result = await _service.GetAllRulesAsync();
            return Ok(result);
        }
    }
}
