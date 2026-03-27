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
        private readonly IDrugService drugService;
        public DrugController(IDrugService _drugService)
        {
            drugService = _drugService;
        }
        
        [HttpGet]
       //    [Route("")]
        public async Task<IActionResult> GetAllDrugs([FromQuery] DrugFilterDTO filter)
        {
            var drugs = await drugService.GetPaginatedResult(filter);
            return Ok(drugs);
        }
        
        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetDrugById(int id)
        {
            var drug = await drugService.GetDrugbyid(id);
            if (drug == null)
            {
                return NotFound(new { errorCode = "DRUG_NOT_FOUND", message = "Drug not found" });
            }
            return Ok(drug);
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