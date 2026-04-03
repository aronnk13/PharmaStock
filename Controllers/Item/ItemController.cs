using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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

        // ✅ CREATE
        [HttpPost("create")]
        public async Task<IActionResult> CreateItem([FromBody] CreateItemDTO dto)
        {
            var itemId = await _itemService.CreateItemAsync(dto);

            return Ok(new
            {
                success = true,
                itemId,
                message = "Item created successfully"
            });
        }

        // ✅ GET BY ID (THIS ENABLES EDIT FLOW)
        [HttpGet("{itemId}")]
        public async Task<IActionResult> GetItemById(int itemId)
        {
            var item = await _itemService.GetItemByIdAsync(itemId);

            if (item == null)
                return NotFound(new { success = false, message = "Item not found" });

            return Ok(item);
        }


        // ✅ UPDATE
        [HttpPut("update/{itemId}")]
        public async Task<IActionResult> UpdateItem(
            int itemId,
            [FromBody] UpdateItemDTO dto)
        {
            if (itemId != dto.ItemId)
                return BadRequest(new
                {
                    success = false,
                    message = "ItemId mismatch"
                });

            try
            {
                await _itemService.UpdateItemAsync(dto);

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
    }
}