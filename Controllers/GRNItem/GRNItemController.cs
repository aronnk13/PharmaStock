using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PharmaStock.Core.DTO.GRNItem;
using PharmaStock.Core.Interfaces.Service;

namespace PharmaStock.Controllers.GRNItem
{
    [ApiController]
    [Route("api/v1/goods-receipt")]
    [Authorize]
    public class GRNItemController : ControllerBase
    {
        private readonly IGRNItemService _service;

        public GRNItemController(IGRNItemService service)
        {
            _service = service;
        }

        [HttpPost("CreateGRNItem")]
        [Authorize(Roles = "Admin,Procurement Officer,Quality Officer")]
        public async Task<IActionResult> Create([FromBody] CreateGRNItemDTO dto)
        {
            try
            {
                var result = await _service.CreateAsync(dto);
                return StatusCode(201, result);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpGet("GetGRNItems")]
        public async Task<IActionResult> Get([FromQuery] GRNItemFilterDTO filter)
        {
            try
            {
                var result = await _service.GetAsync(filter);
                return Ok(result);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpPut("UpdateGRNItem")]
        [Authorize(Roles = "Admin,Quality Officer")]
        public async Task<IActionResult> Update([FromBody] UpdateGRNItemDTO dto)
        {
            try
            {
                var result = await _service.UpdateAsync(dto);
                return Ok(result);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }
    }
}
