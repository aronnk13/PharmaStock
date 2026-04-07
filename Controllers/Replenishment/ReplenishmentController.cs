using Microsoft.AspNetCore.Mvc;
using PharmaStock.Core.DTO.Replenishment;
using PharmaStock.Core.Interfaces.Service;

namespace PharmaStock.Controllers.Replenishment
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReplenishmentController : ControllerBase
    {
        private readonly IReplenishmentService _replenishmentService;
        public ReplenishmentController(IReplenishmentService replenishmentService)
        {
            _replenishmentService = replenishmentService;
        }

// 1. Upsert request

        [HttpPost("UpsertReplenishmentRequest")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpsertReplenishmentRequestAsync([FromBody] UpsertReplenishmentRequestDTO upsertReplenishmentRequestDTO)
        {
            if (upsertReplenishmentRequestDTO == null)
            {
                return BadRequest(new { error = "Request data cannot be null." });
            }

            var response = await _replenishmentService.UpsertReplenishmentRequest(upsertReplenishmentRequestDTO);

            if (response.IsSuccess)
            {
                return Ok(new { message = response.Message });
            }
            else
            {
                return BadRequest(new { error = response.Message });
            }
        }

// 2. Upsert Rule

        [HttpPost("UpsertReplenishmentRule")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpsertReplenishmentRuleAsync([FromBody] UpsertReplenishmentRuleDTO upsertReplenishmentRuleDTO)
        {
            if (upsertReplenishmentRuleDTO == null)
            {
                return BadRequest(new { error = "Request data cannot be null." });
            }

            var response = await _replenishmentService.UpsertReplenishmentRule(upsertReplenishmentRuleDTO);

            if (response.IsSuccess)
            {
                return Ok(new { message = response.Message });
            }
            else
            {
                return BadRequest(new { error = response.Message });
            }
        }

// 3. Delete request

        [HttpDelete("DeleteReplenishmentRequest/{requestId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeleteReplenishmentRequestAsync([FromRoute] int requestId)
        {
            try
            {
                await _replenishmentService.DeleteReplenishmentRequest(requestId);
                return Ok(new { message = "Replenishment request deleted successfully." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

// 4. Delete Rule

        [HttpDelete("DeleteReplenishmentRule/{ruleId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeleteReplenishmentRuleAsync([FromRoute] int ruleId)
        {
            try
            {
                await _replenishmentService.DeleteReplenishmentRule(ruleId);
                return Ok(new { message = "Replenishment rule deleted successfully." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
    }
}