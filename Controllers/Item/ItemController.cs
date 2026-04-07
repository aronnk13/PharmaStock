using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using pharmaStock.Core.DTO.Item;
using PharmaStock.Core.DTO.Item;
using PharmaStock.Core.Interfaces.Service;

namespace PharmaStock.Controllers.Item
{
    [ApiController]
    [Route("api/item")]
    [Authorize(Roles = "Admin")]
    public class ItemController : ControllerBase
    {
        private readonly IItemService _itemService;

        public ItemController(IItemService itemService)
        {
            _itemService = itemService;
        }


        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] ItemDTO itemDTO)
        {
            var itemId = await _itemService.CreateAsync(itemDTO);

            return Ok(new
            {
                success = true,
                itemId,
                message = "Item created successfully"
            });
        }


        [HttpGet("{itemId}")]
        public async Task<IActionResult> GetById(int itemId)
        {
            var item = await _itemService.GetByIdAsync(itemId);

            if (item == null)
                return NotFound(new { success = false, message = "Item not found" });

            return Ok(item);
        }



        [HttpPut("update/{itemId}")]
        public async Task<IActionResult> Update(int itemId, [FromBody] ItemDTO itemDTO)
        {
            try
            {
                await _itemService.UpdateAsync(itemId, itemDTO);

                return Ok(new
                {
                    success = true,
                    message = "Item updated successfully"
                });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new
                {
                    success = false,
                    message = ex.Message
                });
            }
        }
        [HttpGet("GetFiltereditems")]
        public async Task<IActionResult> GetFiltered([FromQuery] ItemFilterDTO filter)
        {
            if (filter.PackSize.HasValue && filter.PackSize.Value <= 0)
            {
                return BadRequest(new { success = false, message = "PackSize must be greater than zero." });
            }

            if (filter.PackSize.HasValue && filter.PackSize.Value > 1000)
            {
                return BadRequest(new { success = false, message = "PackSize cannot exceed 1000 units." });
            }

            if (filter.IsActive != null && !(filter.IsActive == true || filter.IsActive == false))
            {
                return BadRequest(new { success = false, message = "IsActive must be true or false." });
            }

            var items = await _itemService.GetItemsFilteredAsync(filter);
            return Ok(items);
        }
        [HttpDelete]
        [Route("DeleteItem/{itemId}")]
        public async Task<IActionResult> Delete(int itemId)
        {
            if (itemId <= 0)
            {
                return BadRequest(new { success = false, message = "Invalid itemId. Must be greater than zero." });
            }

            var response = await _itemService.DeleteAsync(itemId);

            if (response.IsDeleted)
            {
                return Ok(new { success = true, message = response.Message });
            }

            return BadRequest(new { success = false, message = response.Message });
        }
    }
}