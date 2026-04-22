using System.Security.Claims;
using System.Text.Json;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PharmaStock.Core.DTO;
using PharmaStock.Core.DTO.Transfer;
using PharmaStock.Core.Interfaces.Service;

namespace PharmaStock.Controllers.Transfer
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class TransferOrderController : ControllerBase
    {
        private readonly ITransferOrderService _service;
        private readonly IAuditLogService _auditLogService;

        public TransferOrderController(ITransferOrderService service, IAuditLogService auditLogService)
        {
            _service = service;
            _auditLogService = auditLogService;
        }

        private int GetCurrentUserId()
        {
            var claim = User.FindFirst("userId")?.Value;
            return int.TryParse(claim, out var id) ? id : 0;
        }

        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> CreateTransferOrder([FromBody] CreateTransferOrderDTO dto)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);
                var result = await _service.CreateTransferOrderAsync(dto);

                await _auditLogService.CreateLogAsync(new AuditDto
                {
                    UserId = GetCurrentUserId(),
                    Action = "TRANSFER_ORDER_CREATED",
                    Resource = $"TransferOrder:{result.TransferOrderId}",
                    Metadata = JsonSerializer.Serialize(result)
                });

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("addItem")]
        public async Task<IActionResult> AddTransferItem([FromBody] CreateTransferItemDTO dto)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);
                var result = await _service.AddTransferItemAsync(dto);

                await _auditLogService.CreateLogAsync(new AuditDto
                {
                    UserId = GetCurrentUserId(),
                    Action = "TRANSFER_ITEM_ADDED",
                    Resource = $"TransferOrder:{dto.TransferOrderId}",
                    Metadata = JsonSerializer.Serialize(result)
                });

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("getAll")]
        public async Task<IActionResult> GetAll([FromQuery] TransferOrderFilterDTO filter)
        {
            try
            {
                var result = await _service.GetAllTransferOrdersAsync(filter);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("getItems/{transferOrderId}")]
        public async Task<IActionResult> GetItems(int transferOrderId)
        {
            try
            {
                var result = await _service.GetItemsByTransferOrderIdAsync(transferOrderId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}