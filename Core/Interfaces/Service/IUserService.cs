using PharmaStock.Core.DTO.Register;

namespace PharmaStock.Core.Interfaces.Service
{
    public interface IUserService
    {
        public Task RegisterUser(UserRegistrationDTO userRegistrationDTO, int admindId);
    }
}