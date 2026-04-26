using Microsoft.EntityFrameworkCore;

namespace PharmaStock.Infrastructure.BackgroundJobs
{
    /// <summary>
    /// Runs daily at startup and every 24 hours.
    /// Automatically marks InventoryLots as Expired (Status = 3)
    /// when their ExpiryDate has passed and they are still Active (Status = 1).
    /// </summary>
    public class LotExpiryBackgroundService : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<LotExpiryBackgroundService> _logger;
        private readonly TimeSpan _interval = TimeSpan.FromHours(24);

        public LotExpiryBackgroundService(
            IServiceScopeFactory scopeFactory,
            ILogger<LotExpiryBackgroundService> logger)
        {
            _scopeFactory = scopeFactory;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Lot expiry background job started.");

            while (!stoppingToken.IsCancellationRequested)
            {
                await RunExpiryCheckAsync();
                await Task.Delay(_interval, stoppingToken);
            }
        }

        private async Task RunExpiryCheckAsync()
        {
            try
            {
                using var scope = _scopeFactory.CreateScope();
                var context = scope.ServiceProvider.GetRequiredService<PharmaStock.Models.PharmaStockContext>();

                var today = DateOnly.FromDateTime(DateTime.Today);

                // Fetch all active lots, filter in memory to avoid DateOnly SQL translation issues
                var activeLots = await context.InventoryLots
                    .Where(l => l.Status == 1)
                    .ToListAsync();

                var expiredLots = activeLots
                    .Where(l => l.ExpiryDate <= today)
                    .ToList();

                if (expiredLots.Count == 0)
                {
                    _logger.LogInformation("Lot expiry check: no lots to expire.");
                    return;
                }

                var expiredLotIds = expiredLots.Select(l => l.InventoryLotId).ToList();

                foreach (var lot in expiredLots)
                    lot.Status = 4; // Disposed

                // Zero out all balances for disposed lots
                var balances = await context.InventoryBalances
                    .Where(b => expiredLotIds.Contains(b.InventoryLotId))
                    .ToListAsync();

                foreach (var b in balances)
                    b.QuantityOnHand = 0;

                await context.SaveChangesAsync();

                _logger.LogInformation(
                    "Lot expiry check complete. {Count} lot(s) auto-disposed.", expiredLots.Count);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error running lot expiry check.");
            }
        }
    }
}
