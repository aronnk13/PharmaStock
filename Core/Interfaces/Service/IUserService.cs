using PharmaStock.Core.DTO.Register;

namespace PharmaStock.Core.Interfaces.Service
{
    public interface IUserService
    {
        Task<UpsertResponse> UpsertUser(UpsertUserDTO upsertUserDTO);
        Task<IEnumerable<GetUserDTO>> GetAllUsers();
        Task<GetUserDTO?> GetUserById(int id);
        Task<IEnumerable<GetRoleDTO>> GetAllRoles();
        Task<GetRoleDTO?> GetRoleById(int id);
    }
}