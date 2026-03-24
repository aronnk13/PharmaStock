using Microsoft.AspNetCore.Mvc;
using PharmaStock.Core.DTO.Register;
using Microsoft.AspNetCore.Http;
using PharmaStock.Core.Interfaces.Service;
namespace PharmaStock.Controllers.Admin
{
    [ApiController]
    [Route("api/[controller]")]
    public class AdminController : ControllerBase
    {
        private readonly IUserService _userService;
        public AdminController(IUserService userService)
        {
            _userService = userService;
        }
        [HttpPost("UpsertUser")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpsertUserAsync([FromBody] UpsertUserDTO upsertUserDTO)
        {
            if (upsertUserDTO == null)
            {
                return BadRequest(new { error = "Request data cannot be null." });
            }

            var response = await _userService.UpsertUser(upsertUserDTO);

            if (response.IsSuccess)
            {
                return Ok(new { message = response.Message });
            }
            else
            {
                return BadRequest(new { error = response.Message });
            }
        }
    }
}