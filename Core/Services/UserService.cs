using System.Text.Json;
using PharmaStock.Core.DTO;
using PharmaStock.Core.DTO.Register;
using PharmaStock.Core.Interfaces;
using PharmaStock.Core.Interfaces.Repository;
using PharmaStock.Core.Interfaces.Service;
using PharmaStock.Models;

namespace PharmaStock.Core.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IAuditLogService _auditLogService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserService(IUserRepository userRepository, IAuditLogService auditLogService, IHttpContextAccessor httpContextAccessor)
        {
            _userRepository = userRepository;
            _auditLogService = auditLogService;
            _httpContextAccessor = httpContextAccessor;
        }

        private int GetCurrentUserId() =>
            int.TryParse(_httpContextAccessor.HttpContext?.User.FindFirst("userId")?.Value, out var id) ? id : 0;

        public async Task<UpsertResponse> UpsertUser(UpsertUserDTO upsertUserDTO)
        {
            try
            {
                if (upsertUserDTO.IsCreate)
                {
                    bool exists = await _userRepository.IsUserExistAsync(
                        upsertUserDTO.Username, upsertUserDTO.Email, upsertUserDTO.Phone);

                    if (exists)
                        return new UpsertResponse { IsSuccess = false, Message = "A user with this Username, Email, or Phone already exists." };

                    User user = new User
                    {
                        Username = upsertUserDTO.Username,
                        Email = upsertUserDTO.Email,
                        Phone = upsertUserDTO.Phone,
                        RoleId = upsertUserDTO.RoleId,
                        PasswordHash = "defaultPassword",
                        CreatedBy = upsertUserDTO.AdminName,
                        CreatedOn = DateTime.UtcNow,
                        UpdatedBy = upsertUserDTO.AdminName,
                        UpdatedOn = DateTime.UtcNow,
                        StatusId = upsertUserDTO.StatusId,
                    };
                    await _userRepository.AddAsync(user);

                    await _auditLogService.CreateLogAsync(new AuditDto
                    {
                        UserId = GetCurrentUserId(),
                        Action = "USER_CREATED",
                        Resource = $"User:{user.UserId}",
                        Metadata = JsonSerializer.Serialize(new { upsertUserDTO.Username, upsertUserDTO.RoleId })
                    });

                    return new UpsertResponse { IsSuccess = true, Message = $"User '{upsertUserDTO.Username}' registered successfully." };
                }
                else
                {
                    User? existingUser = await _userRepository.GetByIdAsync(upsertUserDTO.UserId);
                    if (existingUser == null)
                        return new UpsertResponse { IsSuccess = false, Message = $"User with ID {upsertUserDTO.UserId} not found!" };

                    existingUser.Username = upsertUserDTO.Username;
                    existingUser.Email = upsertUserDTO.Email;
                    existingUser.Phone = upsertUserDTO.Phone;
                    existingUser.RoleId = upsertUserDTO.RoleId;
                    existingUser.StatusId = upsertUserDTO.StatusId;
                    existingUser.UpdatedBy = upsertUserDTO.AdminName;
                    existingUser.UpdatedOn = DateTime.UtcNow;
                    _userRepository.Update(existingUser);

                    await _auditLogService.CreateLogAsync(new AuditDto
                    {
                        UserId = GetCurrentUserId(),
                        Action = "USER_UPDATED",
                        Resource = $"User:{existingUser.UserId}",
                        Metadata = JsonSerializer.Serialize(new { upsertUserDTO.Username, upsertUserDTO.RoleId })
                    });

                    return new UpsertResponse { IsSuccess = true, Message = $"User '{upsertUserDTO.Username}' updated successfully." };
                }
            }
            catch (Exception ex)
            {
                return new UpsertResponse { IsSuccess = false, Message = $"An unexpected error occurred: {ex.Message}" };
            }
        }

        public async Task<IEnumerable<GetUserDTO>> GetAllUsers()
        {
            var users = await _userRepository.GetAllUsersWithRoleAsync();

            await _auditLogService.CreateLogAsync(new AuditDto
            {
                UserId = GetCurrentUserId(),
                Action = "USER_LIST_VIEWED",
                Resource = "User:list",
                Metadata = null
            });

            return users.Select(MapToGetUserDTO);
        }

        public async Task<GetUserDTO?> GetUserById(int id)
        {
            var user = await _userRepository.GetUserByIdWithRoleAsync(id);
            if (user == null) return null;

            await _auditLogService.CreateLogAsync(new AuditDto
            {
                UserId = GetCurrentUserId(),
                Action = "USER_VIEWED",
                Resource = $"User:{id}",
                Metadata = null
            });

            return MapToGetUserDTO(user);
        }

        public async Task<IEnumerable<GetRoleDTO>> GetAllRoles()
        {
            var roles = await _userRepository.GetAllRolesAsync();

            await _auditLogService.CreateLogAsync(new AuditDto
            {
                UserId = GetCurrentUserId(),
                Action = "ROLE_LIST_VIEWED",
                Resource = "Role:list",
                Metadata = null
            });

            return roles.Select(r => new GetRoleDTO { RoleId = r.RoleId, RoleType = r.RoleType });
        }

        public async Task<GetRoleDTO?> GetRoleById(int id)
        {
            var role = await _userRepository.GetRoleByIdAsync(id);
            if (role == null) return null;

            await _auditLogService.CreateLogAsync(new AuditDto
            {
                UserId = GetCurrentUserId(),
                Action = "ROLE_VIEWED",
                Resource = $"Role:{id}",
                Metadata = null
            });

            return new GetRoleDTO { RoleId = role.RoleId, RoleType = role.RoleType };
        }

        private static GetUserDTO MapToGetUserDTO(User user) => new GetUserDTO
        {
            UserId = user.UserId,
            Username = user.Username,
            Email = user.Email,
            Phone = user.Phone,
            RoleId = user.RoleId,
            RoleType = user.Role.RoleType,
            StatusId = user.StatusId,
            CreatedOn = user.CreatedOn,
            CreatedBy = user.CreatedBy
        };
    }
}
