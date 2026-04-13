using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PharmaStock.Core.DTO.Transfer;
using PharmaStock.Core.Interfaces.Service;

namespace PharmaStock.Controllers.Transfer
{
    [ApiController]
    [Route("api/[controller]")]
    public class TransferOrderController : ControllerBase
    {
        private readonly ITransferOrderService _service;

        public TransferOrderController(ITransferOrderService service)
        {
            _service = service;
        }

        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> CreateTransferOrder([FromBody] CreateTransferOrderDTO dto)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);
                var result = await _service.CreateTransferOrderAsync(dto);
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
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("getAll")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var result = await _service.GetAllTransferOrdersAsync();
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