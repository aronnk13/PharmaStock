using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PharmaStock.Core.DTO.QCO;
using PharmaStock.Core.Interfaces.Service;

namespace PharmaStock.Controllers.QCO
{
    [ApiController]
    [Authorize(Roles = "QualityComplianceOfficer")]
    [Route("api/recall")]
    public class RecallNoticeController : ControllerBase
    {
        private readonly IRecallNoticeService _service;
        public RecallNoticeController(IRecallNoticeService service) => _service = service;

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
            return Ok(result);
        }

        [HttpPatch("{id}/resolve")]
        public async Task<IActionResult> Resolve(int id)
        {
            var success = await _service.ResolveAsync(id);
            return success ? Ok(new { success = true }) : NotFound();
        }
    }
}
