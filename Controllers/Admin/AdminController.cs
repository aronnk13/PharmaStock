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
                return BadRequest(new { error = "Request data cannot be null." });

            var response = await _userService.UpsertUser(upsertUserDTO);

            if (response.IsSuccess)
                return Ok(new { message = response.Message });

            return BadRequest(new { error = response.Message });
        }

        // ── Users ────────────────────────────────────────────────────────────

        [HttpGet("users")]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _userService.GetAllUsers();
            return Ok(users);
        }

        [HttpGet("users/{id}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            var user = await _userService.GetUserById(id);
            if (user == null)
                return NotFound(new { errorCode = "USER_NOT_FOUND", message = $"User with ID {id} not found." });
            return Ok(user);
        }

        // ── Roles ─────────────────────────────────────────────────────────────

        [HttpGet("roles")]
        public async Task<IActionResult> GetAllRoles()
        {
            var roles = await _userService.GetAllRoles();
            return Ok(roles);
        }

        [HttpGet("roles/{id}")]
        public async Task<IActionResult> GetRoleById(int id)
        {
            var role = await _userService.GetRoleById(id);
            if (role == null)
                return NotFound(new { errorCode = "ROLE_NOT_FOUND", message = $"Role with ID {id} not found." });
            return Ok(role);
        }
    }
}