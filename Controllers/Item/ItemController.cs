using System.Security.Claims;
using System.Text.Json;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PharmaStock.Core.DTO;
using PharmaStock.Core.DTO.Item;
using PharmaStock.Core.Interfaces.Service;

namespace PharmaStock.Controllers.Item
{
    [ApiController]
    [Route("api/items")]
    [Authorize(Roles = "Admin,ProcurementOfficer,Pharmacist,InventoryController,QualityOfficer")]
    public class ItemController : ControllerBase
    {
        private readonly IItemService _itemService;
        private readonly IAuditLogService _auditLogService;

        public ItemController(IItemService itemService, IAuditLogService auditLogService)
        {
            _itemService = itemService;
            _auditLogService = auditLogService;
        }

        private int GetCurrentUserId()
        {
            var claim = User.FindFirst("userId")?.Value;
            return int.TryParse(claim, out var id) ? id : 0;
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

                await _auditLogService.CreateLogAsync(new AuditDto
                {
                    UserId = GetCurrentUserId(),
                    Action = "ITEM_CREATED",
                    Resource = $"Item:{result.ItemId}",
                    Metadata = JsonSerializer.Serialize(result)
                });

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

                await _auditLogService.CreateLogAsync(new AuditDto
                {
                    UserId = GetCurrentUserId(),
                    Action = "ITEM_UPDATED",
                    Resource = $"Item:{itemId}",
                    Metadata = JsonSerializer.Serialize(request)
                });

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

        [HttpPatch]
        [Route("{itemId}/toggle-status")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ToggleStatus([FromRoute] int itemId)
        {
            var success = await _itemService.ToggleStatusAsync(itemId);
            if (!success)
                return NotFound(new { message = "Item not found." });

            await _auditLogService.CreateLogAsync(new AuditDto
            {
                UserId = GetCurrentUserId(),
                Action = "ITEM_STATUS_TOGGLED",
                Resource = $"Item:{itemId}",
                Metadata = JsonSerializer.Serialize(new { itemId })
            });

            return Ok(new { message = "Item status updated." });
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
            {
                await _auditLogService.CreateLogAsync(new AuditDto
                {
                    UserId = GetCurrentUserId(),
                    Action = "ITEM_DELETED",
                    Resource = $"Item:{itemId}",
                    Metadata = JsonSerializer.Serialize(new { itemId })
                });

                return Ok(new { message = response.Message });
            }

            return NotFound(new { message = response.Message });
        }
    }
}
