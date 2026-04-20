using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PharmaStock.Core.Interfaces.Service;

namespace PharmaStock.Controllers.InventoryDashboard
{
    [ApiController]
    [Authorize(Roles = "InventoryController")]
    [Route("api/inventory-dashboard")]
    public class InventoryDashboardController : ControllerBase
    {
        private readonly IInventoryDashboardService _service;

        public InventoryDashboardController(IInventoryDashboardService service)
        {
            _service = service;
        }

        [HttpGet("stats")]
        public async Task<IActionResult> GetStats()
        {
            try
            {
                var result = await _service.GetDashboardStatsAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
