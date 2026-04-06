using System.Security.Claims;
using System.Text.Json;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PharmaStock.Core.DTO;
using PharmaStock.Core.DTO.Bin;
using PharmaStock.Core.Interfaces;
using PharmaStock.Core.Interfaces.Service;

namespace PharmaStock.Controllers.Bin
{
    [ApiController]
    [Route("api/bins")]
    [Authorize(Roles = "Admin")]
    public class BinController : ControllerBase
    {
        private readonly IBinService _binService;
        private readonly IAuditLogService _auditLogService;

        public BinController(IBinService binService, IAuditLogService auditLogService)
        {
            _binService = binService;
            _auditLogService = auditLogService;
        }

        private int GetCurrentUserId()
        {
            var claim = User.FindFirst("userId")?.Value;
            return int.TryParse(claim, out var id) ? id : 0;
        }

        // POST api/bins/CreateBin
        [HttpPost]
        [Route("CreateBin")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
       
        public async Task<IActionResult> CreateBin([FromBody] CreateBinDTO request)
        {
            if (request == null)
                return BadRequest(new { message = "Request body is required." });

            try
            {
                var result = await _binService.CreateBinAsync(request);

                await _auditLogService.CreateLogAsync(new AuditDto
                {
                    UserId = GetCurrentUserId(),
                    Action = "BIN_CREATED",
                    Resource = $"Bin:{result.BinId}",
                    Metadata = JsonSerializer.Serialize(result)
                });

                return CreatedAtAction(nameof(GetBinById), new { binId = result.BinId }, result);
            }
            catch (KeyNotFoundException ex) when (ex.Message == "LOCATION_NOT_FOUND")
            {
                return NotFound(new { errorCode = "LOCATION_NOT_FOUND", message = "Location not found." });
            }
            catch (KeyNotFoundException ex) when (ex.Message == "STORAGE_CLASS_NOT_FOUND")
            {
                return NotFound(new { errorCode = "STORAGE_CLASS_NOT_FOUND", message = "Storage class not found." });
            }
            catch (InvalidOperationException ex) when (ex.Message == "LOCATION_INACTIVE")
            {
                return UnprocessableEntity(new { errorCode = "LOCATION_INACTIVE", message = "Cannot create bin under an inactive location." });
            }
            catch (InvalidOperationException ex) when (ex.Message == "BIN_CODE_DUPLICATE")
            {
                return Conflict(new { errorCode = "BIN_CODE_DUPLICATE", message = $"Bin code '{request.Code}' already exists under location ID {request.LocationId}." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { errorCode = "INTERNAL_ERROR", message = ex.Message });
            }
        }

        // GET api/bins/GetBinById/{binId}
        [HttpGet]
        [Route("GetBinById/{binId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetBinById(int binId)
        {
            if (binId <= 0)
                return BadRequest(new { message = "BinId must be greater than 0." });

            var bin = await _binService.GetBinByIdAsync(binId);
            if (bin == null)
                return NotFound(new { errorCode = "BIN_NOT_FOUND", message = "Bin not found." });
            return Ok(bin);
        }

        // PUT api/bins/UpdateBin/{binId}
        [HttpPut]
        [Route("UpdateBin/{binId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        
        public async Task<IActionResult> UpdateBin([FromRoute] int binId, [FromBody] UpdateBinDTO request)
        {
            if (request == null)
                return BadRequest(new { message = "Request body is required." });

            if (binId <= 0)
                return BadRequest(new { message = "BinId must be greater than 0." });

            try
            {
                var oldBin = await _binService.GetBinByIdAsync(binId);
                if (oldBin == null)
                    return NotFound(new { errorCode = "BIN_NOT_FOUND", message = "Bin not found." });

                var result = await _binService.UpdateBinAsync(binId, request);

                await _auditLogService.CreateLogAsync(new AuditDto
                {
                    UserId = GetCurrentUserId(),
                    Action = "BIN_UPDATED",
                    Resource = $"Bin:{binId}",
                    Metadata = JsonSerializer.Serialize(new { old = oldBin, @new = result })
                });

                return Ok(new { message = "Bin updated successfully." });
            }
            catch (KeyNotFoundException ex) when (ex.Message == "BIN_NOT_FOUND")
            {
                return NotFound(new { errorCode = "BIN_NOT_FOUND", message = "Bin not found." });
            }
            catch (KeyNotFoundException ex) when (ex.Message == "STORAGE_CLASS_NOT_FOUND")
            {
                return NotFound(new { errorCode = "STORAGE_CLASS_NOT_FOUND", message = "Storage class not found." });
            }
            catch (InvalidOperationException ex) when (ex.Message == "BIN_HAS_INVENTORY")
            {
                return Conflict(new { errorCode = "BIN_HAS_INVENTORY", message = "Cannot change storage class or deactivate bin while it has inventory." });
            }
            catch (InvalidOperationException ex) when (ex.Message == "QUARANTINE_HAS_STOCK")
            {
                return Conflict(new { errorCode = "QUARANTINE_HAS_STOCK", message = "Cannot remove quarantine flag while bin still has stock." });
            }
            catch (InvalidOperationException ex) when (ex.Message == "BIN_OPEN_TASKS")
            {
                return Conflict(new { errorCode = "BIN_OPEN_TASKS", message = "Cannot deactivate bin with open put-away tasks." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { errorCode = "INTERNAL_ERROR", message = ex.Message });
            }
        }
    }
}
