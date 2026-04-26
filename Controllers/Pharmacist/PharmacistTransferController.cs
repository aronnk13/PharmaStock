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
        public async Task<IActionResult> GetIncoming([FromQuery] int? locationId)
        {
            var query = _context.TransferOrders
                .Include(t => t.FromLocation)
                .Include(t => t.ToLocation)
                .Include(t => t.StatusNavigation)
                .Include(t => t.TransferItems)
                    .ThenInclude(ti => ti.Item).ThenInclude(i => i.Drug)
                .Include(t => t.TransferItems)
                    .ThenInclude(ti => ti.InventoryLot)
                .AsQueryable();

            if (locationId.HasValue)
                query = query.Where(t => t.ToLocationId == locationId.Value);

            var transfers = await query.OrderByDescending(t => t.CreatedDate).ToListAsync();

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
                    BatchNumber = ti.InventoryLot?.BatchNumber.ToString(),
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

        [HttpPatch("{id}/receive")]
        public async Task<IActionResult> Receive(int id)
        {
            var transfer = await _context.TransferOrders
                .Include(t => t.TransferItems)
                .FirstOrDefaultAsync(t => t.TransferOrderId == id);

            if (transfer == null) return NotFound(new { message = "Transfer order not found." });
            if (transfer.Status != 2) return BadRequest(new { message = "Transfer must be In Progress to receive." });

            foreach (var item in transfer.TransferItems)
            {
                // Deduct from source
                var sourceBalance = await _context.InventoryBalances
                    .FirstOrDefaultAsync(b => b.LocationId == transfer.FromLocationId
                                           && b.InventoryLotId == item.InventoryLotId
                                           && b.ItemId == item.ItemId);
                if (sourceBalance != null)
                {
                    sourceBalance.QuantityOnHand = Math.Max(0, sourceBalance.QuantityOnHand - item.Quantity);
                }

                // Add to destination
                var destBalance = await _context.InventoryBalances
                    .FirstOrDefaultAsync(b => b.LocationId == transfer.ToLocationId
                                           && b.InventoryLotId == item.InventoryLotId
                                           && b.ItemId == item.ItemId);
                if (destBalance != null)
                {
                    destBalance.QuantityOnHand += item.Quantity;
                }
                else
                {
                    // Get first bin at destination location
                    var destBin = await _context.Bins
                        .FirstOrDefaultAsync(b => b.LocationId == transfer.ToLocationId);
                    if (destBin != null)
                    {
                        _context.InventoryBalances.Add(new PharmaStock.Models.InventoryBalance
                        {
                            LocationId = transfer.ToLocationId,
                            BinId = destBin.BinId,
                            ItemId = item.ItemId,
                            InventoryLotId = item.InventoryLotId,
                            QuantityOnHand = item.Quantity,
                            ReservedQty = 0
                        });
                    }
                }
            }

            transfer.Status = 3; // Completed
            await _context.SaveChangesAsync();

            await _auditLogService.CreateLogAsync(new AuditDto
            {
                UserId = GetCurrentUserId(),
                Action = "TRANSFER_RECEIVED",
                Resource = $"TransferOrder:{id}",
                Metadata = JsonSerializer.Serialize(new { transferOrderId = id })
            });

            return Ok(new { success = true });
        }
    }
}
