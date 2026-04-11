using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PharmaStock.Core.DTO.Common;
using PharmaStock.Core.DTO.GRNItem;
using PharmaStock.Core.Interfaces.Service;

namespace PharmaStock.Controllers.GRNItem
{
    [ApiController]
    [Authorize]
    public class GRNItemController : ControllerBase
    {
        private readonly IGRNItemService _service;

        public GRNItemController(IGRNItemService service)
        {
            _service = service;
        }

        [HttpPost]
        [Route("api/v1/goods-receipt/{goodsReceiptId}/items")]
        public async Task<IActionResult> Create(int goodsReceiptId, [FromBody] CreateGRNItemDTO dto)
        {
            try
            {
                var result = await _service.CreateAsync(goodsReceiptId, dto);
                return StatusCode(201, result);
            }
            catch (UnauthorizedAccessException ex)
            {
                return StatusCode(403, ApiErrorResponse.Build(403, "Forbidden", ex.Message, ex.Message));
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ApiErrorResponse.Build(404, "Not Found", ex.Message, ex.Message));
            }
            catch (InvalidOperationException ex)
            {
                var status = ex.Message == "GRI_ERR_008" ? 422 : 409;
                return StatusCode(status, ApiErrorResponse.Build(status, status == 422 ? "Unprocessable Entity" : "Conflict", ex.Message, ex.Message));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ApiErrorResponse.Build(400, "Bad Request", ex.Message, ex.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiErrorResponse.Build(500, "Internal Server Error", "SYS_ERR_003", ex.Message));
            }
        }

        [HttpGet]
        [Route("api/v1/goods-receipt/{goodsReceiptId}/items")]
        public async Task<IActionResult> GetAll(int goodsReceiptId, [FromQuery] GRNItemFilterDTO filter)
        {
            try
            {
                var result = await _service.GetAllAsync(
                    goodsReceiptId, filter);
                return Ok(result);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ApiErrorResponse.Build(404, "Not Found", ex.Message, ex.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiErrorResponse.Build(500, "Internal Server Error", "SYS_ERR_003", ex.Message));
            }
        }

        [HttpGet]
        [Route("api/v1/goods-receipt/{goodsReceiptId}/items/{goodsReceiptItemId}")]
        public async Task<IActionResult> GetById(int goodsReceiptId, int goodsReceiptItemId)
        {
            try
            {
                var result = await _service.GetByIdAsync(
                    goodsReceiptId, goodsReceiptItemId);
                return Ok(result);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ApiErrorResponse.Build(404, "Not Found", ex.Message, ex.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiErrorResponse.Build(500, "Internal Server Error", "SYS_ERR_003", ex.Message));
            }
        }

        [HttpPut]
        [Route("api/v1/goods-receipt/{goodsReceiptId}/items/{goodsReceiptItemId}")]
        public async Task<IActionResult> Update(int goodsReceiptId, int goodsReceiptItemId, [FromBody] UpdateGRNItemDTO dto)
        {
            try
            {
                var result = await _service.UpdateAsync(goodsReceiptId, goodsReceiptItemId, dto);
                return Ok(result);
            }
            catch (UnauthorizedAccessException ex)
            {
                return StatusCode(403, ApiErrorResponse.Build(403, "Forbidden", ex.Message, ex.Message));
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ApiErrorResponse.Build(404, "Not Found", ex.Message, ex.Message));
            }
            catch (InvalidOperationException ex)
            {
                var status = ex.Message == "GRI_ERR_031" ? 422 : 409;
                return StatusCode(status, ApiErrorResponse.Build(status, status == 422 ? "Unprocessable Entity" : "Conflict", ex.Message, ex.Message));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ApiErrorResponse.Build(400, "Bad Request", ex.Message, ex.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiErrorResponse.Build(500, "Internal Server Error", "SYS_ERR_003", ex.Message));
            }
        }

        [HttpDelete]
        [Route("api/v1/goods-receipt/{goodsReceiptId}/items/{goodsReceiptItemId}")]
        public async Task<IActionResult> Delete(int goodsReceiptId, int goodsReceiptItemId, [FromBody] DeleteGRNItemDTO dto)
        {
            try
            {
                var result = await _service.DeleteAsync(goodsReceiptId, goodsReceiptItemId, dto);
                return Ok(result);
            }
            catch (UnauthorizedAccessException ex)
            {
                return StatusCode(403, ApiErrorResponse.Build(403, "Forbidden", ex.Message, ex.Message));
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ApiErrorResponse.Build(404, "Not Found", ex.Message, ex.Message));
            }
            catch (InvalidOperationException ex)
            {
                return StatusCode(409, ApiErrorResponse.Build(409, "Conflict", ex.Message, ex.Message));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ApiErrorResponse.Build(400, "Bad Request", ex.Message, ex.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiErrorResponse.Build(500, "Internal Server Error", "SYS_ERR_003", ex.Message));
            }
        }
    }
}
