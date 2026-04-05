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
        public async Task<IActionResult> Update(int itemId,[FromBody] ItemDTO itemDTO)
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

        [HttpDelete]
        [Route("DeleteItem/{itemId}")]
        public async Task<IActionResult> Delete(int itemId)
        {
            var userIdClaim = User.FindFirst("userId");
            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out var requestingUserId))
            {
                return Unauthorized(new { success = false, message = "User ID claim missing or invalid" });
            }

            var response = await _itemService.DeleteAsync(itemId, requestingUserId);

            if (response.IsDeleted)
            {
                return Ok(new { success = true, message = response.Message });
            }

            return BadRequest(new { success = false, message = response.Message });
        }
    }
}