using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PharmaStock.Core.Interfaces.Service;

namespace PharmaStock.Controllers.ExpiryWatch
{
    [ApiController]
    [Authorize(Roles = "InventoryController,QualityComplianceOfficer,Pharmacist")]
    [Route("api/expirywatch")]
    public class ExpiryWatchController : ControllerBase
    {
        private readonly IExpiryWatchService _service;

        public ExpiryWatchController(IExpiryWatchService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _service.GetAllAsync();
            return Ok(result);
        }

        [HttpGet("active")]
        public async Task<IActionResult> GetActive()
        {
            var result = await _service.GetActiveWatchesAsync();
            return Ok(result);
        }

        // days param now filters by actual ExpiryDate proximity, not the watch threshold
        [HttpGet("near-expiry")]
        public async Task<IActionResult> GetNearExpiry([FromQuery] int days = 500)
        {
            var result = await _service.GetNearExpiryAsync(days);
            return Ok(result);
        }
    }
}
