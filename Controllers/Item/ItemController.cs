using Microsoft.AspNetCore.Mvc;
using PharmaStock.Core.DTO.Item;
using PharmaStock.Core.Interfaces.Service;

namespace PharmaStock.Controllers.Item
{
    [ApiController]
    [Route("api/item")]
    public class ItemController : ControllerBase
    {
        private readonly IItemService _itemService;

        public ItemController(IItemService itemService)
        {
            _itemService = itemService;
        }

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

        [HttpPut("update")]
        public async Task<IActionResult> UpdateItem([FromBody] UpdateItemDTO dto)
        {
            try
            {
                await _itemService.UpdateItemAsync(dto);

                return Ok(new
                {
                    success = true,
                    message = "Item updated successfully"
                });
            }
            catch (Exception ex)
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
