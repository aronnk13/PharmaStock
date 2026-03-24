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
        public async Task<UpsertResponse> UpsertUser(UpsertUserDTO upsertUserDTO)
        {
            try
            {
                if (upsertUserDTO.IsCreate)
                {
                    bool exists = await _userRepository.IsUserExistAsync(
                   upsertUserDTO.Username,
                   upsertUserDTO.Email,
                   upsertUserDTO.Phone,
                   upsertUserDTO.IsCreate ? null : upsertUserDTO.UserId
               );

                    if (exists)
                    {
                        return new UpsertResponse
                        {
                            IsSuccess = false,
                            Message = "A user with this Username, Email, or Phone already exists."
                        };
                    }
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
                        StatusId = true,
                    };
                    await _userRepository.AddAsync(user);

                    return new UpsertResponse { IsSuccess = true, Message = $"User '{upsertUserDTO.Username}' registered successfully." };
                }
                else
                {
                    User? existingUser = await _userRepository.GetByIdAsync(upsertUserDTO.UserId);
                    if (existingUser == null)
                    {
                        return new UpsertResponse { IsSuccess = false, Message = $"User with ID {upsertUserDTO.UserId} not found!" };
                    }

                    existingUser.Username = upsertUserDTO.Username;
                    existingUser.Email = upsertUserDTO.Email;
                    existingUser.Phone = upsertUserDTO.Phone;
                    existingUser.RoleId = upsertUserDTO.RoleId;

                    existingUser.UpdatedBy = upsertUserDTO.AdminName;
                    existingUser.UpdatedOn = DateTime.UtcNow;

                    _userRepository.Update(existingUser);

                    return new UpsertResponse { IsSuccess = true, Message = $"User '{upsertUserDTO.Username}' updated successfully." };
                }
            }
            catch (Exception ex)
            {
                return new UpsertResponse
                {
                    IsSuccess = false,
                    Message = $"An unexpected error occurred: {ex.Message}"
                };
            }
        }
    }
}