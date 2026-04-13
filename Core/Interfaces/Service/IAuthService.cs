using PharmaStock.Core.DTO.Auth;

namespace PharmaStock.Core.Interfaces.Service
{
    public interface IAuthService
    {
        Task<LoginResponseDTO> LoginAsync(LoginDTO dto);
    }
}
