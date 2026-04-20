using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PharmaStock.Core.DTO.Item;
using PharmaStock.Core.Interfaces.Service;

namespace PharmaStock.Controllers.Item
{
    [ApiController]
    [Route("api/items")]
    [Authorize(Roles = "Admin")]
    public class ItemController : ControllerBase
    {
        private readonly IItemService _itemService;

        public ItemController(IItemService itemService)
        {
            _itemService = itemService;
        }

        [HttpPost]
        [Route("CreateItem")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateItem([FromBody] ItemDTO request)
        {
            if (request == null)
                return BadRequest(new { message = "Request body is required." });

            try
            {
                var result = await _itemService.CreateAsync(request);
                return CreatedAtAction(nameof(GetItemById), new { itemId = result.ItemId }, result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { errorCode = "INTERNAL_ERROR", message = ex.Message });
            }
        }

        [HttpGet]
        [Route("GetItemById/{itemId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetItemById(int itemId)
        {
            if (itemId <= 0)
                return BadRequest(new { message = "ItemId must be greater than 0." });

            var item = await _itemService.GetByIdAsync(itemId);
            if (item == null)
                return NotFound(new { errorCode = "ITEM_NOT_FOUND", message = "Item not found." });

            return Ok(item);
        }

        [HttpGet]
        [Route("GetAllItems")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllItems()
        {
            var items = await _itemService.GetAllAsync();
            return Ok(items);
        }

        [HttpPut]
        [Route("UpdateItem/{itemId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateItem([FromRoute] int itemId, [FromBody] ItemDTO request)
        {
            if (request == null)
                return BadRequest(new { message = "Request body is required." });

            if (itemId <= 0)
                return BadRequest(new { message = "ItemId must be greater than 0." });

            try
            {
                await _itemService.UpdateAsync(itemId, request);
                return Ok(new { message = "Item updated successfully." });
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new { errorCode = "ITEM_NOT_FOUND", message = "Item not found." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { errorCode = "INTERNAL_ERROR", message = ex.Message });
            }
        }

        [HttpDelete]
        [Route("DeleteItem/{itemId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteItem([FromRoute] int itemId)
        {
            if (itemId <= 0)
                return BadRequest(new { message = "ItemId must be greater than 0." });

            var response = await _itemService.DeleteAsync(itemId);

            if (response.IsDeleted)
                return Ok(new { message = response.Message });

            return NotFound(new { message = response.Message });
        }
    }
}
