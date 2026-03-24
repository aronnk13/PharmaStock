using PharmaStock.Core.DTO.Register;

namespace PharmaStock.Core.Interfaces.Service
{
    public interface IUserService
    {
        public Task<UpsertResponse> UpsertUser(UpsertUserDTO upsertUserDTO);

    }
}