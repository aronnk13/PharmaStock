using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PharmaStock.Core.Interfaces.Repository;

namespace PharmaStock.Controllers.Pharmacist
{
    [ApiController]
    [Authorize(Roles = "Pharmacist")]
    [Route("api/pharmacist-inventory")]
    public class PharmacistInventoryController : ControllerBase
    {
        private readonly IInventoryBalanceRepository _repo;
        public PharmacistInventoryController(IInventoryBalanceRepository repo) => _repo = repo;

        [HttpGet]
        public async Task<IActionResult> GetByLocation([FromQuery] int? locationId)
        {
            var items = locationId.HasValue
                ? await _repo.GetByLocationAsync(locationId.Value)
                : await _repo.GetAllWithDetailsAsync();

            var today = DateOnly.FromDateTime(DateTime.Today);

            var result = items
                .Where(b => b.InventoryLot != null
                         && b.InventoryLot.Status == 1          // Active lots only
                         && b.InventoryLot.ExpiryDate > today)   // Not expired (today still dispensable)
                .Select(b => new
                {
                    b.InventoryBalanceId,
                    b.LocationId,
                    LocationName = b.Location?.Name,
                    b.ItemId,
                    ItemName = b.Item?.Drug?.GenericName,
                    b.InventoryLotId,
                    BatchNumber = b.InventoryLot?.BatchNumber,
                    ExpiryDate = b.InventoryLot?.ExpiryDate,
                    b.QuantityOnHand,
                    b.ReservedQty,
                    AvailableQty = b.QuantityOnHand - b.ReservedQty
                });

            return Ok(result);
        }
    }
}
