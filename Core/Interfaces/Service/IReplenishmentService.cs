using PharmaStock.Core.DTO.Replenishment;

namespace PharmaStock.Core.Interfaces.Service
{
    public interface IReplenishmentService
    {
        // Requests
        Task<IEnumerable<ReplenishmentRequestDTO>> GetAllRequestsAsync();
        Task<IEnumerable<ReplenishmentRequestDTO>> GetRequestsByStatusAsync(int status);
        Task<ReplenishmentRequestDTO?> GetRequestByIdAsync(int id);
        Task<ReplenishmentRequestDTO> CreateRequestAsync(CreateReplenishmentRequestDTO dto);
        Task<bool> UpdateRequestStatusAsync(int id, int status);

        // Rules
        Task<IEnumerable<ReplenishmentRuleDTO>> GetAllRulesAsync();
        Task<ReplenishmentRuleDTO> CreateRuleAsync(CreateReplenishmentRuleDTO dto);
        Task<bool> UpdateRuleAsync(int id, CreateReplenishmentRuleDTO dto);
        Task<bool> DeleteRuleAsync(int id);

        // Auto replenishment
        Task<RunCheckResultDTO> RunReplenishmentCheckAsync();
        Task<ConvertToTransferOrderResultDTO?> ConvertToTransferOrderAsync(int reqId);
    }
}
