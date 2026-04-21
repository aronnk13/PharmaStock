using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using PharmaStock.Core.DTO.Auth;
using PharmaStock.Core.Interfaces.Repository;
using PharmaStock.Core.Interfaces.Service;
using PharmaStock.Models;

namespace PharmaStock.Core.Services
{
    public class AuthService : IAuthService
    {
        private readonly IAuthRepository _authRepository;
        private readonly IConfiguration  _configuration;

        public AuthService(IAuthRepository authRepository, IConfiguration configuration)
        {
            _authRepository = authRepository;
            _configuration  = configuration;
        }

        public async Task<LoginResponseDTO> LoginAsync(LoginDTO dto)
        {
            // 1. Fetch user (with role) from DB
            var user = await _authRepository.GetUserByUsernameAsync(dto.Username);
            if (user == null)
                throw new UnauthorizedAccessException("INVALID_USERNAME");

            // 2. Password check (plain-text comparison — demo only)
            if (dto.Password != user.PasswordHash)
                throw new UnauthorizedAccessException("INVALID_PASSWORD");

            // 3. Build JWT and return response
            var token = GenerateJwtToken(user);

            return new LoginResponseDTO
            {
                Token  = token,
                UserId = user.UserId,
                Role   = user.Role.RoleType
            };
        }

        private string GenerateJwtToken(User user)
        {
            // IMPORTANT: Both "role" (custom) and ClaimTypes.Role (standard) are included.
            // Program.cs sets RoleClaimType = ClaimTypes.Role so [Authorize(Roles = "X")]
            // reads ClaimTypes.Role. The frontend reads the "role" custom claim from the JWT.
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Username),
                new Claim("userId",           user.UserId.ToString()),
                new Claim("role",             user.Role.RoleType),          // for frontend
                new Claim(ClaimTypes.Role,    user.Role.RoleType),          // for [Authorize]
            };

            var jwtKey = _configuration["Jwt:Key"]
                         ?? throw new InvalidOperationException("JWT Key not configured");

            var key   = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer:            _configuration["Jwt:Issuer"],
                audience:          _configuration["Jwt:Issuer"],
                claims:            claims,
                expires:           DateTime.UtcNow.AddHours(8),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
