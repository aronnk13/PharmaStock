using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PharmaStock.Core.Interfaces;

namespace PharmaStock.Controllers.Drug
{
    [ApiController]
    [Route("api/[controller]")]
    public class DrugController : ControllerBase
    {
        private readonly IDrugService _drugService;

        public DrugController(IDrugService drugService)
        {
            _drugService = drugService;
        }

        [HttpDelete]
        [Route("DeleteDrug/{DrugId}")]
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