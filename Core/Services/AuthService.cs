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
        private readonly IConfiguration _configuration;

        public AuthService(IAuthRepository authRepository, IConfiguration configuration)
        {
            _authRepository = authRepository;
            _configuration = configuration;
        }

        public async Task<LoginResponseDTO> LoginAsync(LoginDTO dto)
        {
            // 1. Fetch user with role from DB
            var user = await _authRepository.GetUserByUsernameAsync(dto.Username);
            if (user == null)
                throw new UnauthorizedAccessException("INVALID_USERNAME");

            // 2. Active status check
            if (!user.StatusId)
                throw new UnauthorizedAccessException("ACCOUNT_INACTIVE");

            // 3. Password check
            if (dto.Password != user.PasswordHash)
                throw new UnauthorizedAccessException("INVALID_PASSWORD");

            // 3. Generate JWT token
            var token = GenerateJwtToken(user);

            return new LoginResponseDTO
            {
                Token = token,
                UserId = user.UserId,
                Role = user.Role.RoleType
            };
        }

        private string GenerateJwtToken(User user)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Username),
                new Claim("userId", user.UserId.ToString()),
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
