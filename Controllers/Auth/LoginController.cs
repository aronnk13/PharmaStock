using Microsoft.AspNetCore.Mvc;
using PharmaStock.Models;
using BCrypt.Net;
using Microsoft.EntityFrameworkCore;
using PharmaStock.Core.DTO.Auth;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace PharmaStock.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LoginController : ControllerBase
    {
        private readonly PharmaStockContext _context;
        private readonly IConfiguration _configuration;

        public LoginController(PharmaStockContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginDTO request)
        {
            // Find user by username
            // Console.WriteLine(request.Username);
            var user = await _context.Users.Include(u => u.Role).FirstOrDefaultAsync(u => u.Username == request.Username);

            if (user == null)
                return Unauthorized(new { message = "Invalid username or password" });

            // Compare password with stored hash
            // bool isPasswordValid = BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash);

            if (request.Password != user.PasswordHash)
                return Unauthorized(new { message = "Invalid username or password" });

            // JWT Part
            var claims = new[]
           {
                new Claim(JwtRegisteredClaimNames.Sub, user.Username),
                new Claim("userId", user.UserId.ToString()),
                new Claim("role", user.Role.RoleType)
            };

            var jwtKey = _configuration["Jwt:Key"] 
                         ?? throw new InvalidOperationException("JWT Key not configured");
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
               issuer: _configuration["Jwt:Issuer"],
               audience: _configuration["Jwt:Issuer"],
               claims: claims,
               expires: DateTime.Now.AddHours(1),
               signingCredentials: creds);

            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

            return Ok(new { token = tokenString, userId = user.UserId, role = user.Role.RoleType });
        }
    }
}
