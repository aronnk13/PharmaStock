using PharmaStock.Core.DTO.ExpiryWatch;

namespace PharmaStock.Core.Interfaces.Service
{
    public interface IExpiryWatchService
    {
        Task<IEnumerable<ExpiryWatchDTO>> GetAllAsync();
        Task<IEnumerable<ExpiryWatchDTO>> GetActiveWatchesAsync();
        Task<IEnumerable<ExpiryWatchDTO>> GetNearExpiryAsync(int daysThreshold);
    }
}
