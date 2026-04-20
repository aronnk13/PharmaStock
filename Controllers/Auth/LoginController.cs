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
            new Claim(JwtRegisteredClaimNames.Sub, user.Username),
            new Claim("userId", user.UserId.ToString()),
            new Claim(ClaimTypes.Role, user.Role.RoleType),
            new Claim("role", user.Role.RoleType)
            };

            var jwtKey = _configuration["Jwt:Key"] ?? throw new InvalidOperationException("JWT Key not configured");

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Issuer"],
                claims: claims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
