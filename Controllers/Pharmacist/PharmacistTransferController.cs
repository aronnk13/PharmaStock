using System.Security.Claims;
using System.Text.Json;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PharmaStock.Core.DTO;
using PharmaStock.Core.DTO.Pharmacist;
using PharmaStock.Core.Interfaces.Service;
using PharmaStock.Models;

namespace PharmaStock.Controllers.Pharmacist
{
    [ApiController]
    [Authorize(Roles = "Pharmacist")]
    [Route("api/pharmacist-transfer")]
    public class PharmacistTransferController : ControllerBase
    {
        private readonly PharmaStockContext _context;
        private readonly IAuditLogService _auditLogService;

        public PharmacistTransferController(PharmaStockContext context, IAuditLogService auditLogService)
        {
            _context = context;
            _auditLogService = auditLogService;
        }

        private int GetCurrentUserId()
        {
            var claim = User.FindFirst("userId")?.Value;
            return int.TryParse(claim, out var id) ? id : 0;
        }

        [HttpGet("incoming")]
        public async Task<IActionResult> GetIncoming([FromQuery] int locationId)
        {
            var transfers = await _context.TransferOrders
                .Include(t => t.FromLocation)
                .Include(t => t.ToLocation)
                .Include(t => t.StatusNavigation)
                .Include(t => t.TransferItems)
                    .ThenInclude(ti => ti.Item).ThenInclude(i => i.Drug)
                .Include(t => t.TransferItems)
                    .ThenInclude(ti => ti.InventoryLot)
                .Where(t => t.ToLocationId == locationId)
                .OrderByDescending(t => t.CreatedDate)
                .ToListAsync();

            var result = transfers.Select(t => new IncomingTransferDTO
            {
                TransferOrderId = t.TransferOrderId,
                FromLocationId = t.FromLocationId,
                FromLocationName = t.FromLocation?.Name,
                ToLocationId = t.ToLocationId,
                ToLocationName = t.ToLocation?.Name,
                CreatedDate = t.CreatedDate,
                Status = t.Status,
                StatusName = t.StatusNavigation?.Status,
                Items = t.TransferItems.Select(ti => new TransferItemDTO
                {
                    TransferItemId = ti.TransferItemId,
                    ItemId = ti.ItemId,
                    ItemName = ti.Item?.Drug?.GenericName,
                    InventoryLotId = ti.InventoryLotId,
                    BatchNumber = ti.InventoryLot?.BatchNumber,
                    Quantity = ti.Quantity
                }).ToList()
            });

            return Ok(result);
        }

        [HttpPatch("{id}/confirm")]
        public async Task<IActionResult> Confirm(int id)
        {
            var transfer = await _context.TransferOrders.FindAsync(id);
            if (transfer == null) return NotFound();
            transfer.Status = 2;
            await _context.SaveChangesAsync();

            await _auditLogService.CreateLogAsync(new AuditDto
            {
                UserId = GetCurrentUserId(),
                Action = "TRANSFER_CONFIRMED",
                Resource = $"TransferOrder:{id}",
                Metadata = JsonSerializer.Serialize(new { transferOrderId = id })
            });

            return Ok(new { success = true });
        }
    }
}
