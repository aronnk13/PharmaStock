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
    [Route("api/recall")]
    public class RecallNoticeController : ControllerBase
    {
        private readonly IRecallNoticeService _service;
        private readonly IAuditLogService _auditLogService;

        public RecallNoticeController(IRecallNoticeService service, IAuditLogService auditLogService)
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
        public async Task<IActionResult> Create([FromBody] CreateRecallNoticeDTO dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var result = await _service.CreateAsync(dto);

            await _auditLogService.CreateLogAsync(new AuditDto
            {
                UserId = GetCurrentUserId(),
                Action = "RECALL_CREATED",
                Resource = $"RecallNotice:{result.RecallNoticeId}",
                Metadata = JsonSerializer.Serialize(result)
            });

            return Ok(result);
        }

        [HttpPatch("{id}/close")]
        public async Task<IActionResult> Close(int id)
        {
            var success = await _service.CloseAsync(id);
            if (!success) return NotFound();

            await _auditLogService.CreateLogAsync(new AuditDto
            {
                UserId = GetCurrentUserId(),
                Action = "RECALL_CLOSED",
                Resource = $"RecallNotice:{id}",
                Metadata = JsonSerializer.Serialize(new { recallNoticeId = id })
            });

            return Ok(new { success = true });
        }
    }
}
