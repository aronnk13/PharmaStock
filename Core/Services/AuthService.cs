using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using Microsoft.IdentityModel.Tokens;
using PharmaStock.Core.DTO;
using PharmaStock.Core.DTO.Auth;
using PharmaStock.Core.Interfaces;
using PharmaStock.Core.Interfaces.Repository;
using PharmaStock.Core.Interfaces.Service;
using PharmaStock.Models;

namespace PharmaStock.Core.Services
{
    public class AuthService : IAuthService
    {
        private readonly IAuthRepository _authRepository;
        private readonly IConfiguration _configuration;
        private readonly IAuditLogService _auditLogService;

        public AuthService(IAuthRepository authRepository, IConfiguration configuration, IAuditLogService auditLogService)
        {
            _authRepository = authRepository;
            _configuration = configuration;
            _auditLogService = auditLogService;
        }

        public async Task<LoginResponseDTO> LoginAsync(LoginDTO dto)
        {
            var user = await _authRepository.GetUserByUsernameAsync(dto.Username);
            if (user == null)
                throw new UnauthorizedAccessException("INVALID_USERNAME");

            if (dto.Password != user.PasswordHash)
                throw new UnauthorizedAccessException("INVALID_PASSWORD");

            var token = GenerateJwtToken(user);

            var response = new LoginResponseDTO
            {
                Token = token,
                UserId = user.UserId,
                Role = user.Role.RoleType
            };

            await _auditLogService.CreateLogAsync(new AuditDto
            {
                UserId = user.UserId,
                Action = "USER_LOGIN",
                Resource = $"User:{user.UserId}",
                Metadata = JsonSerializer.Serialize(new { user.UserId, Role = user.Role.RoleType })
            });

            return response;
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
