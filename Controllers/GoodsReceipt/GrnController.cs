using System.Security.Claims;
using System.Text.Json;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PharmaStock.Core.DTO;
using PharmaStock.Core.DTO.GoodsReceipt;
using PharmaStock.Core.Interfaces;
using PharmaStock.Core.Interfaces.Service;

namespace PharmaStock.Controllers.GoodsReceipt
{
    [ApiController]
    [Route("api/GoodsReceipts")]
    [Authorize(Roles = "Admin")]
    public class GrnController : ControllerBase
    {
        private readonly IGrnService _grnService;
        private readonly IAuditLogService _auditLogService;

        public GrnController(IGrnService grnService, IAuditLogService auditLogService)
        {
            _grnService = grnService;
            _auditLogService = auditLogService;
        }

        private int GetCurrentUserId()
        {
            var claim = User.FindFirst("userId")?.Value;
            return int.TryParse(claim, out var id) ? id : 0;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> CreateGrn([FromBody] CreateGrnDTO request)
        {
            if (request == null)
                return BadRequest(new { message = "Request body is required." });

            try
            {
                var result = await _grnService.CreateGrnAsync(request);

                await _auditLogService.CreateLogAsync(new AuditDto
                {
                    UserId = GetCurrentUserId(),
                    Action = "GRN_CREATED",
                    Resource = $"GoodsReceipt:{result.GoodsReceiptId}",
                    Metadata = JsonSerializer.Serialize(result)
                });

                return CreatedAtAction(nameof(GetGrnById), new { grnId = result.GoodsReceiptId }, result);
            }
            catch (KeyNotFoundException ex) when (ex.Message == "PO_NOT_FOUND")
            {
                return NotFound(new { errorCode = "PO_NOT_FOUND", message = "Purchase order not found." });
            }
            catch (InvalidOperationException ex) when (ex.Message == "PO_NOT_RECEIVABLE")
            {
                return Conflict(new { errorCode = "PO_NOT_RECEIVABLE", message = "Purchase order status must be Approved or PartiallyReceived." });
            }
            catch (ArgumentException ex) when (ex.Message == "INVALID_DATE")
            {
                return BadRequest(new { errorCode = "INVALID_DATE", message = "Received date cannot be a future date." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { errorCode = "INTERNAL_ERROR", message = ex.Message });
            }
        }

        [HttpGet("{grnId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetGrnById(int grnId)
        {
            if (grnId <= 0)
                return BadRequest(new { message = "GrnId must be greater than 0." });

            var grn = await _grnService.GetGrnByIdAsync(grnId);
            if (grn == null)
                return NotFound(new { errorCode = "GRN_NOT_FOUND", message = "Goods receipt not found." });

            return Ok(grn);
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllGrns([FromQuery] GrnFilterDTO filter)
        {
            var result = await _grnService.GetAllGrnsAsync(filter);
            return Ok(result);
        }

        [HttpPut("{grnId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> UpdateGrn([FromRoute] int grnId, [FromBody] UpdateGrnDTO request)
        {
            if (request == null)
                return BadRequest(new { message = "Request body is required." });

            if (grnId <= 0)
                return BadRequest(new { message = "GrnId must be greater than 0." });

            try
            {
                var oldGrn = await _grnService.GetGrnByIdAsync(grnId);
                if (oldGrn == null)
                    return NotFound(new { errorCode = "GRN_NOT_FOUND", message = "Goods receipt not found." });

                var result = await _grnService.UpdateGrnAsync(grnId, request);
                
                // Determine audit action based on what changed
                var action = request.StatusId == 2 ? "GRN_POSTED"
                           : request.StatusId == 3 ? "GRN_REJECTED"
                           : "GRN_UPDATED";

                await _auditLogService.CreateLogAsync(new AuditDto
                {
                    UserId = GetCurrentUserId(),
                    Action = action,
                    Resource = $"GoodsReceipt:{grnId}",
                    Metadata = JsonSerializer.Serialize(new { old = oldGrn, @new = result })
                });

                return Ok(result);
            }
            catch (KeyNotFoundException ex) when (ex.Message == "GRN_NOT_FOUND")
            {
                return NotFound(new { errorCode = "GRN_NOT_FOUND", message = "Goods receipt not found." });
            }
            catch (ArgumentException ex) when (ex.Message == "INVALID_DATE")
            {
                return BadRequest(new { errorCode = "INVALID_DATE", message = "Received date cannot be a future date." });
            }
            catch (ArgumentException ex) when (ex.Message == "INVALID_STATUS")
            {
                return BadRequest(new { errorCode = "INVALID_STATUS", message = "StatusId must be 2 (Post) or 3 (Reject)." });
            }
            catch (InvalidOperationException ex) when (ex.Message == "GRN_NOT_EDITABLE")
            {
                return Conflict(new { errorCode = "GRN_NOT_EDITABLE", message = "GRN can only be edited when status is Open." });
            }
            catch (InvalidOperationException ex) when (ex.Message == "GRN_NOT_OPEN")
            {
                return Conflict(new { errorCode = "GRN_NOT_OPEN", message = "GRN must be Open to post." });
            }
            catch (InvalidOperationException ex) when (ex.Message == "GRN_NO_ITEMS")
            {
                return Conflict(new { errorCode = "GRN_NO_ITEMS", message = "GRN must have at least one item before posting." });
            }
            catch (InvalidOperationException ex) when (ex.Message == "GRN_NOT_POSTED")
            {
                return Conflict(new { errorCode = "GRN_NOT_POSTED", message = "GRN must be Posted to reject." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { errorCode = "INTERNAL_ERROR", message = ex.Message });
            }
        }
    }
}
