using System.Security.Claims;
using System.Text.Json;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PharmaStock.Core.DTO;
using PharmaStock.Core.DTO.Pharmacist;
using PharmaStock.Core.Interfaces.Service;
using PharmaStock.Models;
using Microsoft.EntityFrameworkCore;

namespace PharmaStock.Controllers.Pharmacist
{
    [ApiController]
    [Authorize(Roles = "Pharmacist")]
    [Route("api/dispense")]
    public class DispenseController : ControllerBase
    {
        private readonly IDispenseService _service;
        private readonly PharmaStockContext _context;
        private readonly IAuditLogService _auditLogService;

        public DispenseController(IDispenseService service, PharmaStockContext context, IAuditLogService auditLogService)
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

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] int? locationId)
        {
            if (locationId.HasValue)
            {
                var byLocation = await _service.GetByLocationAsync(locationId.Value);
                return Ok(byLocation);
            }
            var result = await _service.GetAllAsync();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _service.GetByIdAsync(id);
            return result == null ? NotFound() : Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateDispenseRefDTO dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var result = await _service.CreateAsync(dto);

            await _auditLogService.CreateLogAsync(new AuditDto
            {
                UserId = GetCurrentUserId(),
                Action = "DISPENSE_CREATED",
                Resource = $"Dispense:{result.DispenseRefId}",
                Metadata = JsonSerializer.Serialize(result)
            });

            return Ok(result);
        }

        [HttpGet("destination-types")]
        public async Task<IActionResult> GetDestinationTypes()
        {
            var types = await _context.DestinationTypes
                .Select(d => new { d.DestinationTypeId, d.Type })
                .ToListAsync();
            return Ok(types);
        }
    }
}
