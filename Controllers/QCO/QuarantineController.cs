using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PharmaStock.Core.DTO.QCO;
using PharmaStock.Core.Interfaces.Service;

namespace PharmaStock.Controllers.QCO
{
    [ApiController]
    [Authorize(Roles = "QualityComplianceOfficer")]
    [Route("api/quarantine")]
    public class QuarantineController : ControllerBase
    {
        private readonly IQuarantineService _service;
        public QuarantineController(IQuarantineService service) => _service = service;

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
            return Ok(result);
        }

        [HttpPatch("{id}/release")]
        public async Task<IActionResult> Release(int id)
        {
            var success = await _service.ReleaseAsync(id);
            return success ? Ok(new { success = true }) : NotFound();
        }

        [HttpPatch("{id}/dispose")]
        public async Task<IActionResult> Dispose(int id)
        {
            var success = await _service.DisposeAsync(id);
            return success ? Ok(new { success = true }) : NotFound();
        }
    }
}
