using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PharmaStock.Core.DTO;
using PharmaStock.Core.Interfaces;

namespace PharmaStock.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuditController(IAuditLogService auditLogService) : ControllerBase
    {
        [HttpPost]
        [Route("CreateAudit")]
        public async Task<IActionResult> CreateAudit(AuditDto dto)
        {
            await auditLogService.CreateLogAsync(dto);
            return Ok("Success");
        }
    }
}