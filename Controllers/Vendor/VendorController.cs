using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PharmaStock.Core.DTO.Vendor;
using PharmaStock.Core.Interfaces.Service;

namespace PharmaStock.Controllers.Vendor
{
    [ApiController]
    [Authorize(Roles = "ProcurementOfficer")] // Restrict access to procurement officers
    [Route("api/vendor")]
    public class VendorController : ControllerBase
    {
        private readonly IVendorService _vendorService;

        public VendorController(IVendorService vendorService)
        {
            _vendorService = vendorService;
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create(VendorDTO dto)
        {
            try
            {
                return Ok(await _vendorService.CreateAsync(dto));
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
        }

        [HttpGet("{vendorId}")]
        public async Task<IActionResult> GetById(int vendorId)
        {
            var vendor = await _vendorService.GetByIdAsync(vendorId);
            return vendor == null ? NotFound() : Ok(vendor);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] bool includeInactive = false)
        {
            return Ok(await _vendorService.GetAllAsync(includeInactive));
        }

        [HttpPut("update/{vendorId}")]
        public async Task<IActionResult> Update(int vendorId, VendorDTO dto)
        {
            await _vendorService.UpdateAsync(vendorId, dto);
            return Ok(new { success = true });
        }

        [HttpPatch("{vendorId}/status")]
        public async Task<IActionResult> ChangeStatus(int vendorId, bool isActive)
        {
            await _vendorService.SetStatusAsync(vendorId, isActive);
            return Ok(new { success = true });
        }

        [HttpDelete("{vendorId}")]
        public async Task<IActionResult> Delete(int vendorId)
        {
            await _vendorService.DeleteAsync(vendorId);
            return Ok(new { success = true });
        }
    }
}
