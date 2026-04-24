using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PharmaStock.Core.Interfaces.Service;

namespace PharmaStock.Controllers.InventoryBalance
{
    [ApiController]
    [Authorize(Roles = "InventoryController")]
    [Route("api/inventorybalance")]
    public class InventoryBalanceController : ControllerBase
    {
        private readonly IInventoryBalanceService _service;

        public InventoryBalanceController(IInventoryBalanceService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _service.GetAllAsync();
            return Ok(result);
        }

        [HttpGet("location/{locationId}")]
        public async Task<IActionResult> GetByLocation(int locationId)
        {
            var result = await _service.GetByLocationAsync(locationId);
            return Ok(result);
        }

        [HttpGet("item/{itemId}")]
        public async Task<IActionResult> GetByItem(int itemId)
        {
            var result = await _service.GetByItemAsync(itemId);
            return Ok(result);
        }

        [HttpGet("low-stock")]
        public async Task<IActionResult> GetLowStock([FromQuery] int threshold = 10)
        {
            var result = await _service.GetLowStockAsync(threshold);
            return Ok(result);
        }
    }
}
