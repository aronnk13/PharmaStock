using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PharmaStock.Core.DTO.Drug;
using PharmaStock.Core.Interfaces.Service;
using PharmaStock.Core.Services;

namespace PharmaStock.Controllers.Drug
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
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
                return Ok(new { message = response.Message });  // NoContent() => 204 delete success
            }
            
            return BadRequest(new { message = response.Message });
        }
    }
}