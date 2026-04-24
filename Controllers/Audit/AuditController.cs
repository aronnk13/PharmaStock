using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PharmaStock.Core.DTO;
using PharmaStock.Core.DTO.Audit;
using PharmaStock.Core.Interfaces.Service;

namespace PharmaStock.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuditController(IAuditLogService auditLogService) : ControllerBase
    {
        [HttpGet]
        [Route("GetAllAuditLogs")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllAuditLogs()
        {
            var logs = await auditLogService.GetAllAsync();
            return Ok(logs);
        }

        [HttpPost]
        [Route("CreateAudit")]
        public async Task<ActionResult<AuditLog>> CreateAudit(AuditDto dto)
        {
            var res=await auditLogService.CreateLogAsync(dto);
            if (!res.Result)
            {
                return StatusCode(500,res);
            }
            return Ok(res);
        }
    }
}