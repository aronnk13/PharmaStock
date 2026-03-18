using PharmaStock.Core.DTO.Register;
using PharmaStock.Core.Interfaces.Repository;
using PharmaStock.Core.Interfaces.Service;
using PharmaStock.Infrastructure.Repositories;
using PharmaStock.Models;

namespace PharmaStock.Core.Services
{
    public class UserService(IUserRepository userRepository) : IUserService
    {
        public async Task RegisterUser(UserRegistrationDTO userRegistrationDTO, int admindId)
        {
            // User admin = await userRepository.GetByIdAsync(admindId);
            
            User user = new User{
                Username = userRegistrationDTO.Username,
                Email = userRegistrationDTO.Email,
                Phone = userRegistrationDTO.Phone,
                RoleId = userRegistrationDTO.RoleId,
                PasswordHash = "defaultPassword",
                CreatedBy = "admin.Username",
                CreatedOn = DateTime.UtcNow,
                UpdatedOn = DateTime.UtcNow,
                UpdatedBy = "admin.Username",
                StatusId = true,
            };
            await userRepository.AddAsync(user);
        }
    }
}