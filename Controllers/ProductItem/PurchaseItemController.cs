using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PharmaStock.Core.DTO.Item;
using PharmaStock.Core.Interfaces.Service;

namespace PharmaStock.Controllers.ProductItem
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class PurchaseItemController : ControllerBase
    {
        private readonly IPurchaseItemService _service;

        public PurchaseItemController(IPurchaseItemService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllPurchaseItems()
        {
            try
            {
                return Ok(await _service.GetAllPIAsync());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreatePurchaseItem(CreatePurchaseItemDTO dto)
        {
            try
            {
                return Ok(await _service.AddPIAsync(dto));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePurchaseItem(int id, UpdatePurchaseItemDTO dto)
        {
            try
            {
                return Ok(await _service.UpdatePIAsync(id, dto));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePurchaseItem(int id)
        {
            try
            {
                await _service.DeletePIAsync(id);
                return Ok(new { message = "PurchaseItem deleted successfully." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message, inner = ex.InnerException?.Message });
            }
        }
    }
}
