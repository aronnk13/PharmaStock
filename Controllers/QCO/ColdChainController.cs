using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PharmaStock.Core.DTO.QCO;
using PharmaStock.Core.Interfaces.Service;

namespace PharmaStock.Controllers.QCO
{
    [ApiController]
    [Authorize]
    [Route("api/coldchain")]
    public class ColdChainController : ControllerBase
    {
        private readonly IColdChainLogService _service;

        public ColdChainController(IColdChainLogService service) => _service = service;

        [HttpGet("logs")]
        public async Task<IActionResult> GetLogs()
        {
            var result = await _service.GetAllAsync();
            return Ok(result);
        }

        [HttpPost("logs")]
        public async Task<IActionResult> CreateLog([FromBody] CreateColdChainLogDTO dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var result = await _service.CreateAsync(dto);
            return Ok(result);
        }
    }
}
