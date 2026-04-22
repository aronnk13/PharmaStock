using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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

        public DrugController(IDrugService drugService)
        {
            _drugService = drugService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllDrugs([FromQuery] DrugFilterDTO filter)
        {
            var drugs = await _drugService.GetPaginatedResult(filter);
            return Ok(drugs);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetDrugById(int id)
        {
            var drug = await _drugService.GetDrugbyid(id);
            if (drug == null)
                return NotFound(new { errorCode = "DRUG_NOT_FOUND", message = "Drug not found." });
            return Ok(drug);
        }

        [HttpPost("CreateDrug")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> CreateDrug([FromBody] CreateDrugDTO request)
        {
            if (request == null)
                return BadRequest(new { message = "Request body is required." });

            try
            {
                var result = await _drugService.CreateDrug(request);
                return CreatedAtAction(nameof(GetDrugById), new { id = result.DrugId }, result);
            }
            catch (InvalidOperationException ex) when (ex.Message == "DRUG_DUPLICATE")
            {
                return Conflict(new { errorCode = "DRUG_DUPLICATE", message = "This drug already exists." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { errorCode = "INTERNAL_ERROR", message = ex.Message });
            }
        }

        [HttpPut("UpdateDrug/{drugId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateDrug([FromRoute] int drugId, [FromBody] UpdateDrugDTO request)
        {
            if (request == null)
                return BadRequest(new { message = "Request body is required." });
            if (drugId <= 0)
                return BadRequest(new { message = "DrugId must be greater than 0." });

            try
            {
                var success = await _drugService.UpdateDrug(drugId, request);
                if (!success)
                    return NotFound(new { errorCode = "DRUG_NOT_FOUND", message = "Drug not found." });
                return Ok(new { message = "Drug updated successfully." });
            }
            catch (InvalidOperationException ex) when (ex.Message == "DRUG_DUPLICATE")
            {
                return Conflict(new { errorCode = "DRUG_DUPLICATE", message = "This drug already exists." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { errorCode = "INTERNAL_ERROR", message = ex.Message });
            }
        }

        [HttpDelete("DeleteDrug/{drugId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteDrug([FromRoute] int drugId)
        {
            if (drugId <= 0)
                return BadRequest(new { message = "DrugId must be greater than 0." });

            var response = await _drugService.DeleteDrug(drugId);

            if (response.IsDeleted)
                return Ok(new { message = response.Message });

            return NotFound(new { message = response.Message });
        }
    }
}
