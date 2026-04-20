using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PharmaStock.Core.DTO.Item;
using PharmaStock.Core.Interfaces.Service;

namespace PharmaStock.Controllers.ProductItem
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class PurchaseItemController : ControllerBase
    {
        private readonly IPurchaseItemService service;
        public PurchaseItemController(IPurchaseItemService _service)
        {
            service = _service;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllPurchaseItems()
        {
            try
            {
                var res = await service.GetAllPIAsync();
                return Ok(res);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> CreatePurchaseItem(CreatePurchaseItemDTO dto)
        {
            try
            {
                var res = await service.AddPIAsync(dto);
                return Ok(res);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> UpdatePurchaseItem(int id, UpdatePurchaseItemDTO dto)
        {
            try
            {
                var res = await service.UpdatePIAsync(id, dto);
                return Ok(res);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> DeletePurchaseItem(int id)
        {
            try
            {
                await service.DeletePIAsync(id);
                return Ok("PurchaseItem deleted successfully");
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    message = ex.Message,
                    inner = ex.InnerException?.Message,
                    stack = ex.StackTrace
                });
            }
        }
    }
}