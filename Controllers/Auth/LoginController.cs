using Microsoft.AspNetCore.Mvc;
using PharmaStock.Core.DTO.Auth;
using PharmaStock.Core.Interfaces.Service;

namespace PharmaStock.Controllers.Auth
{
    [ApiController]
    [Route("api/[controller]")]
    public class LoginController : ControllerBase
    {
        private readonly IAuthService _authService;

        public LoginController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginDTO request)
        {
            if (request == null)
                return BadRequest(new { message = "Invalid request body." });

            try
            {
                var response = await _authService.LoginAsync(request);
                return Ok(new
                {
                    token  = response.Token,
                    userId = response.UserId,
                    role   = response.Role
                });
            }
            catch (UnauthorizedAccessException ex) when (ex.Message == "INVALID_USERNAME")
            {
                return Unauthorized(new { message = "Invalid username." });
            }
            catch (UnauthorizedAccessException ex) when (ex.Message == "INVALID_PASSWORD")
            {
                return Unauthorized(new { message = "Invalid password." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }
    }
}
