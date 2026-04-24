using System.Security.Claims;
using System.Text.Json;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PharmaStock.Core.DTO;
using PharmaStock.Core.DTO.Drug;
using PharmaStock.Core.Interfaces.Service;

namespace PharmaStock.Controllers.Drug
{
    [ApiController]
    [Route("api/v1/drugs")]
    [Authorize(Roles = "Admin")]
    public class DrugController : ControllerBase
    {
        private readonly IDrugService _drugService;
        private readonly IAuditLogService _auditLogService;
        public DrugController(IDrugService drugService, IAuditLogService auditLogService)
        {
            _drugService = drugService;
            _auditLogService = auditLogService;
        }

        private int GetCurrentUserId()
        {
            var claim = User.FindFirst("userId")?.Value;
            return int.TryParse(claim, out var id) ? id : 0;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllDrugs([FromQuery] DrugFilterDTO filter)
        {
            var drugs = await _drugService.GetPaginatedResult(filter);
            return Ok(drugs);
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetDrugById(int id)
        {
            var drug = await _drugService.GetDrugbyid(id);
            if (drug == null)
            {
                return NotFound(new { errorCode = "DRUG_NOT_FOUND", message = "Drug not found" });
            }
            return Ok(drug);
        }

        [HttpPost]
        [Route("CreateDrug")]
        public async Task<IActionResult> CreateDrug([FromBody] CreateDrugDTO request)
        {
            if (request == null)
            {
                return BadRequest("Drug data is required.");
            }
            try
            {
                var result = await _drugService.CreateDrug(request);

                await _auditLogService.CreateLogAsync(new AuditDto
                {
                    UserId = GetCurrentUserId(),
                    Action = "DRUG_CREATED",
                    Resource = $"Drug:{result.DrugId}",
                    Metadata = JsonSerializer.Serialize(result)
                });

                return CreatedAtAction(nameof(GetDrugById), new { id = result.DrugId }, result);
            }
            catch (InvalidOperationException ex) when (ex.Message == "DRUG_DUPLICATE")
            {
                return Conflict(new { errorCode = "DRUG_DUPLICATE", message = "This drug already exists." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut]
        [Route("UpdateDrug/{DrugId}")]
        public async Task<IActionResult> UpdateDrug([FromRoute] int DrugId, [FromBody] UpdateDrugDTO request)
        {
            if (request == null)
            {
                return BadRequest(new { message = "Request body is required." });
            }
            if (DrugId <= 0 )
            {
                return BadRequest(new { message = "DrugId must be greater than 0." });
            }
            try
            {
                var success = await _drugService.UpdateDrug(DrugId,request);
                if (!success) return NotFound();

                await _auditLogService.CreateLogAsync(new AuditDto
                {
                    UserId = GetCurrentUserId(),
                    Action = "DRUG_UPDATED",
                    Resource = $"Drug:{DrugId}",
                    Metadata = JsonSerializer.Serialize(request)
                });

                return Ok(new { message = "Drug updated successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete]
        [Route("DeleteDrug/{DrugId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> DeleteDrug([FromRoute] int DrugId)
        {
            var response = await _drugService.DeleteDrug(DrugId);
            if (response.IsDeleted)
            {
                await _auditLogService.CreateLogAsync(new AuditDto
                {
                    UserId = GetCurrentUserId(),
                    Action = "DRUG_DELETED",
                    Resource = $"Drug:{DrugId}",
                    Metadata = JsonSerializer.Serialize(new { drugId = DrugId })
                });

                return Ok(new { message = response.Message });  // NoContent() => 204 delete success
            }

            return BadRequest(new { message = response.Message });
        }
    }
}