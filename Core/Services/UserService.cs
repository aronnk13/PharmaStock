using PharmaStock.Core.DTO.Register;
using PharmaStock.Core.Interfaces.Repository;
using PharmaStock.Core.Interfaces.Service;
using PharmaStock.Models;

namespace PharmaStock.Core.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public async Task UpsertUser(UpsertUserDTO upsertUserDTO)
        {
            if (upsertUserDTO == null) throw new ArgumentNullException(nameof(upsertUserDTO), "Request data cannot be null.");
            User? admin = await _userRepository.GetByIdAsync(upsertUserDTO.AdminId) ?? throw new Exception("Invalid Admin ID. Registration blocked.");

            if (upsertUserDTO.IsCreate)
            {
                User user = new User
                {
                    Username = upsertUserDTO.Username,
                    Email = upsertUserDTO.Email,
                    Phone = upsertUserDTO.Phone,
                    RoleId = upsertUserDTO.RoleId,
                    PasswordHash = "defaultPassword",
                    CreatedBy = admin.Username,
                    CreatedOn = DateTime.UtcNow,
                    UpdatedBy = admin.Username,
                    UpdatedOn = DateTime.UtcNow,
                    StatusId = true,
                };
                await _userRepository.AddAsync(user);
            }
            else
            {
                User? existingUser = await _userRepository.GetByIdAsync(upsertUserDTO.UserId);
                if (existingUser == null) throw new Exception($"User with ID {upsertUserDTO.UserId} not found!");

                existingUser.Username = upsertUserDTO.Username;
                existingUser.Email = upsertUserDTO.Email;
                existingUser.Phone = upsertUserDTO.Phone;
                existingUser.RoleId = upsertUserDTO.RoleId;

                existingUser.UpdatedBy = admin.Username;
                existingUser.UpdatedOn = DateTime.UtcNow;

                _userRepository.Update(existingUser);
            }
        }
    }
}