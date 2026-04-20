using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PharmaStock.Core.Interfaces.Service;

namespace PharmaStock.Controllers.Pharmacist
{
    [ApiController]
    [Authorize(Roles = "Pharmacist")]
    [Route("api/pharmacist-dashboard")]
    public class PharmacistDashboardController : ControllerBase
    {
        private readonly IPharmacistDashboardService _service;
        public PharmacistDashboardController(IPharmacistDashboardService service) => _service = service;

        [HttpGet("stats")]
        public async Task<IActionResult> GetStats([FromQuery] int locationId = 1)
        {
            var result = await _service.GetDashboardAsync(locationId);
            return Ok(result);
        }
    }
}
