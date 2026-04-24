using System.Security.Claims;
using System.Text.Json;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PharmaStock.Core.DTO;
using PharmaStock.Core.DTO.Vendor;
using PharmaStock.Core.Interfaces.Service;

namespace PharmaStock.Controllers.Vendor
{
    [ApiController]
    [Authorize(Roles = "ProcurementOfficer")]
    [Route("api/vendor")]
    public class VendorController : ControllerBase
    {
        private readonly IVendorService _vendorService;
        private readonly IAuditLogService _auditLogService;

        public VendorController(IVendorService vendorService, IAuditLogService auditLogService)
        {
            _vendorService = vendorService;
            _auditLogService = auditLogService;
        }

        private int GetCurrentUserId()
        {
            var claim = User.FindFirst("userId")?.Value;
            return int.TryParse(claim, out var id) ? id : 0;
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create(VendorDTO dto)
        {
            try
            {
                var result = await _vendorService.CreateAsync(dto);

                await _auditLogService.CreateLogAsync(new AuditDto
                {
                    UserId = GetCurrentUserId(),
                    Action = "VENDOR_CREATED",
                    Resource = $"Vendor:{result.VendorId}",
                    Metadata = JsonSerializer.Serialize(result)
                });

                return Ok(result);
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
        public async Task<IActionResult> GetAll(
            [FromQuery] bool includeInactive = false,
            [FromQuery] string? name = null)
        {
            return Ok(await _vendorService.GetAllAsync(includeInactive, name));
        }

  
        [HttpPut("update/{vendorId}")]
        public async Task<IActionResult> Update(int vendorId, VendorDTO dto)
        {
            await _vendorService.UpdateAsync(vendorId, dto);

            await _auditLogService.CreateLogAsync(new AuditDto
            {
                UserId = GetCurrentUserId(),
                Action = "VENDOR_UPDATED",
                Resource = $"Vendor:{vendorId}",
                Metadata = JsonSerializer.Serialize(dto)
            });

            return Ok(new { success = true });
        }


        [HttpDelete("{vendorId}")]
        public async Task<IActionResult> Delete(int vendorId)
        {
            await _vendorService.DeleteAsync(vendorId);

            await _auditLogService.CreateLogAsync(new AuditDto
            {
                UserId = GetCurrentUserId(),
                Action = "VENDOR_DELETED",
                Resource = $"Vendor:{vendorId}",
                Metadata = JsonSerializer.Serialize(new { vendorId })
            });

            return Ok(new { success = true });
        }
    }
}
