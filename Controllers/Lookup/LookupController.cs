using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PharmaStock.Models;

namespace PharmaStock.Controllers.Lookup
{
    [ApiController]
    [Route("api/lookup")]
    [Authorize(Roles = "Admin")]
    public class LookupController : ControllerBase
    {
        private readonly PharmaStockContext _context;

        public LookupController(PharmaStockContext context)
        {
            _context = context;
        }

        [HttpGet("drug-forms")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetDrugForms()
        {
            var result = await _context.DrugForms
                .Select(d => new { d.DrugFormId, d.Form })
                .ToListAsync();
            return Ok(result);
        }

        [HttpGet("control-classes")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetControlClasses()
        {
            var result = await _context.ControlClasses
                .Select(c => new { c.ControlClassId, c.Class })
                .ToListAsync();
            return Ok(result);
        }

        [HttpGet("storage-classes")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetStorageClasses()
        {
            var result = await _context.BinStorageClasses
                .Select(s => new { s.BinStorageClassId, s.StorageClass })
                .ToListAsync();
            return Ok(result);
        }

        [HttpGet("uoms")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetUoMs()
        {
            var result = await _context.UoMs
                .Select(u => new { u.UoMid, u.Code, u.Description })
                .ToListAsync();
            return Ok(result);
        }
    }
}
