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
                return BadRequest(new { message = "Invalid Client Request" });

            try
            {
                var response = await _authService.LoginAsync(request);
                return Ok(response);
            }
            catch (UnauthorizedAccessException ex) when (ex.Message == "INVALID_USERNAME")
            {
                return Unauthorized(new { message = "Invalid Username." });
            }
            catch (UnauthorizedAccessException ex) when (ex.Message == "INVALID_PASSWORD")
            {
                return Unauthorized(new { message = "Invalid Password." });
            }
        }
    }
}
