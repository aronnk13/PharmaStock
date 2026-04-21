using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PharmaStock.Core.DTO.QCO;
using PharmaStock.Core.Interfaces.Repository;

namespace PharmaStock.Controllers.QCO
{
    [ApiController]
    [Authorize(Roles = "QualityComplianceOfficer")]
    [Route("api/stock-adjustment")]
    public class StockAdjustmentController : ControllerBase
    {
        private readonly IStockAdjustmentRepository _repo;
        public StockAdjustmentController(IStockAdjustmentRepository repo) => _repo = repo;

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var items = await _repo.GetAllWithDetailsAsync();
            var result = items.Select(s => new StockAdjustmentDTO
            {
                StockAdjustmentId  = s.StockAdjustmentId,
                LocationId         = s.LocationId,
                LocationName       = s.Location?.Name,
                ItemId             = s.ItemId,
                ItemName           = s.Item?.Drug?.GenericName,
                InventoryLotId     = s.InventoryLotId,
                BatchNumber        = s.InventoryLot?.BatchNumber,
                QuantityDelta      = s.QuantityDelta,
                ReasonCode         = s.ReasonCode,
                ReasonDescription  = s.ReasonCodeNavigation?.Description,
                ApprovedBy         = s.ApprovedBy,
                ApprovedByName     = s.ApprovedByNavigation?.Username,
                PostedDate         = s.PostedDate
            });
            return Ok(result);
        }
    }
}
