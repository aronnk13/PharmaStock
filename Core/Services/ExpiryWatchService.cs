using PharmaStock.Core.DTO.ExpiryWatch;
using PharmaStock.Core.Interfaces.Repository;
using PharmaStock.Core.Interfaces.Service;

namespace PharmaStock.Core.Services
{
    public class ExpiryWatchService : IExpiryWatchService
    {
        private readonly IExpiryWatchRepository _repo;

        public ExpiryWatchService(IExpiryWatchRepository repo)
        {
            _repo = repo;
        }

        public async Task<IEnumerable<ExpiryWatchDTO>> GetAllAsync()
        {
            var watches = await _repo.GetAllAsync();
            return watches.Select(Map);
        }

        public async Task<IEnumerable<ExpiryWatchDTO>> GetActiveWatchesAsync()
        {
            var watches = await _repo.GetActiveWatchesAsync();
            return watches.Select(Map);
        }

        public async Task<IEnumerable<ExpiryWatchDTO>> GetNearExpiryAsync(int daysThreshold)
        {
            var watches = await _repo.GetNearExpiryAsync(daysThreshold);
            return watches.Select(Map);
        }

        private static ExpiryWatchDTO Map(Models.ExpiryWatch e)
        {
            var today = DateOnly.FromDateTime(DateTime.UtcNow);
            var expiryDate = e.InventoryLot?.ExpiryDate ?? DateOnly.MinValue;
            var actualDaysRemaining = expiryDate.DayNumber - today.DayNumber;

            return new ExpiryWatchDTO
            {
                ExpiryWatchId = e.ExpiryWatchId,
                InventoryLotId = e.InventoryLotId,
                BatchNumber = e.InventoryLot?.BatchNumber ?? 0,
                ItemId = e.InventoryLot?.ItemId ?? 0,
                ItemName = e.InventoryLot?.Item?.Drug?.GenericName ?? "Unknown",
                ExpiryDate = expiryDate,
                DaysToExpire = actualDaysRemaining,
                FlagDate = e.FlagDate,
                Status = e.Status
            };
        }
    }
}
