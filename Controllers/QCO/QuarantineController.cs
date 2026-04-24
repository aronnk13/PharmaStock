using System.Security.Claims;
using System.Text.Json;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PharmaStock.Core.DTO;
using PharmaStock.Core.DTO.QCO;
using PharmaStock.Core.Interfaces.Service;

namespace PharmaStock.Controllers.QCO
{
    [ApiController]
    [Authorize(Roles = "QualityOfficer")]
    [Route("api/quarantine")]
    public class QuarantineController : ControllerBase
    {
        private readonly IQuarantineService _service;
        private readonly IAuditLogService _auditLogService;

        public QuarantineController(IQuarantineService service, IAuditLogService auditLogService)
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
            return result == null ? NotFound() : Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateQuarantineActionDTO dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var result = await _service.CreateAsync(dto);

            await _auditLogService.CreateLogAsync(new AuditDto
            {
                UserId = GetCurrentUserId(),
                Action = "QUARANTINE_CREATED",
                Resource = $"Quarantine:{result.QuarantaineActionId}",
                Metadata = JsonSerializer.Serialize(result)
            });

            return Ok(result);
        }

        [HttpPatch("{id}/release")]
        public async Task<IActionResult> Release(int id)
        {
            var success = await _service.ReleaseAsync(id);
            if (!success) return NotFound();

            await _auditLogService.CreateLogAsync(new AuditDto
            {
                UserId = GetCurrentUserId(),
                Action = "QUARANTINE_RELEASED",
                Resource = $"Quarantine:{id}",
                Metadata = JsonSerializer.Serialize(new { quarantineActionId = id })
            });

            return Ok(new { success = true });
        }

        [HttpPatch("{id}/dispose")]
        public async Task<IActionResult> Dispose(int id)
        {
            var success = await _service.DisposeAsync(id);
            if (!success) return NotFound();

            await _auditLogService.CreateLogAsync(new AuditDto
            {
                UserId = GetCurrentUserId(),
                Action = "QUARANTINE_DISPOSED",
                Resource = $"Quarantine:{id}",
                Metadata = JsonSerializer.Serialize(new { quarantineActionId = id })
            });

            return Ok(new { success = true });
        }
    }
}
