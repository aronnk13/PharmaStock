using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PharmaStock.Core.DTO.InventoryLot;
using PharmaStock.Core.Interfaces.Service;

namespace PharmaStock.Controllers.InventoryLot
{
    [ApiController]
    [Authorize(Roles = "InventoryController")]
    [Route("api/inventorylot")]
    public class InventoryLotController : ControllerBase
    {
        private readonly IInventoryLotService _service;

        public InventoryLotController(IInventoryLotService service)
        {
            _service = service;
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create(InventoryLotDTO dto)
        {
            return Ok(await _service.CreateAsync(dto));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var lot = await _service.GetByIdAsync(id);
            return lot == null ? NotFound() : Ok(lot);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _service.GetAllAsync());
        }

        [HttpPut("update/{id}")]
        public async Task<IActionResult> Update(int id, InventoryLotDTO dto)
        {
            await _service.UpdateAsync(id, dto);
            return Ok(new { success = true });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.DeleteAsync(id);
            return Ok(new { success = true });
        }
    }
}