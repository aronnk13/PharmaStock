using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PharmaStock.Core.Interfaces.Service;

namespace PharmaStock.Controllers.QCO
{
    [ApiController]
    [Authorize(Roles = "QualityComplianceOfficer")]
    [Route("api/qco-dashboard")]
    public class QCODashboardController : ControllerBase
    {
        private readonly IQCODashboardService _service;
        public QCODashboardController(IQCODashboardService service) => _service = service;

        [HttpGet("stats")]
        public async Task<IActionResult> GetStats()
        {
            var result = await _service.GetDashboardAsync();
            return Ok(result);
        }
    }
}
