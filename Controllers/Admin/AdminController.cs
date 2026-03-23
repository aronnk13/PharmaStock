using Microsoft.AspNetCore.Mvc;
using PharmaStock.Core.DTO.Register;
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
        public async Task<IActionResult> UpsertUserAsync([FromBody] UpsertUserDTO upsertUserDTO)
        {
            if (upsertUserDTO == null)
            {
                return BadRequest(new { error = "Request data cannot be null." });
            }
           try
            {
                await _userService.UpsertUser(upsertUserDTO);
                return Ok(new { message = $"User '{upsertUserDTO.Username}' registered successfully." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
    }
}