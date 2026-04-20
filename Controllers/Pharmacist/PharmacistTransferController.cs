using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PharmaStock.Core.DTO.Pharmacist;
using PharmaStock.Models;

namespace PharmaStock.Controllers.Pharmacist
{
    [ApiController]
    [Authorize(Roles = "Pharmacist")]
    [Route("api/pharmacist-transfer")]
    public class PharmacistTransferController : ControllerBase
    {
        private readonly PharmaStockContext _context;
        public PharmacistTransferController(PharmaStockContext context) => _context = context;

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
            return Ok(new { success = true });
        }
    }
}
