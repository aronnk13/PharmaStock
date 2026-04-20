using PharmaStock.Core.DTO.Replenishment;

namespace PharmaStock.Core.Interfaces.Service
{
    public interface IReplenishmentService
    {
        Task<IEnumerable<ReplenishmentRequestDTO>> GetAllRequestsAsync();
        Task<IEnumerable<ReplenishmentRequestDTO>> GetRequestsByStatusAsync(int status);
        Task<ReplenishmentRequestDTO?> GetRequestByIdAsync(int id);
        Task<ReplenishmentRequestDTO> CreateRequestAsync(CreateReplenishmentRequestDTO dto);
        Task<bool> UpdateRequestStatusAsync(int id, int status);
        Task<IEnumerable<ReplenishmentRuleDTO>> GetAllRulesAsync();
    }
}
