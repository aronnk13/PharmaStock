using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PharmaStock.Core.DTO.Drug;
using PharmaStock.Core.Interfaces.Service;
using PharmaStock.Core.Services;

namespace PharmaStock.Controllers.Drug
{
    [ApiController]
    [Route("api/v1/drugs")]
    public class DrugController : ControllerBase
    {
        private readonly IDrugService _drugService;
        public DrugController(IDrugService drugService)
        {
            _drugService = drugService;
        }

        [HttpGet]
        //    [Route("")]
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

        [HttpPost("CreateDrug")]
        public async Task<IActionResult> CreateDrug([FromBody] CreateDrugDTO request)
        {
            try
            {
                var result = await _drugService.CreateDrug(request);
                // Returns 201 Created with the location of the new resource
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

        [HttpPut("UpdateDrug/{DrugId}")]
        public async Task<IActionResult> UpdateDrug([FromRoute] int DrugId, [FromBody] UpdateDrugDTO request)
        {
            if (DrugId != request.DrugId)
                return BadRequest(new { message = "ID Mismatch" });

            try
            {
                var success = await _drugService.UpdateDrug(request);
                if (!success) return NotFound();

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
        public async Task<IActionResult> DeleteDrug([FromRoute] int DrugId)
        {
            var response = await _drugService.DeleteDrug(DrugId);
            if (response.IsDeleted)
            {
                return Ok(new { message = response.Message });  // NoContent() => 204 delete success
            }

            return BadRequest(new { message = response.Message });
        }
    }
}