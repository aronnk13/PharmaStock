using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PharmaStock.Models;

namespace PharmaStock.Controllers.Location
{
    [ApiController]
    [Authorize]
    [Route("api/locations")]
    public class LocationController : ControllerBase
    {
        private readonly PharmaStockContext _context;

        public LocationController(PharmaStockContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var locations = await _context.Locations
                .Where(l => l.StatusId == true)
                .Select(l => new { l.LocationId, l.Name, l.LocationTypeId })
                .ToListAsync();
            return Ok(locations);
        }
    }
}
