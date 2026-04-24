using PharmaStock.Core.Interfaces.Service;

namespace PharmaStock.Infrastructure.BackgroundJobs
{
    /// <summary>
    /// Runs the replenishment check every hour (configurable via "ReplenishmentJob:IntervalMinutes").
    /// Uses IServiceScopeFactory because IReplenishmentService is scoped, not singleton.
    /// </summary>
    public class ReplenishmentBackgroundService : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<ReplenishmentBackgroundService> _logger;
        private readonly TimeSpan _interval;

        public ReplenishmentBackgroundService(
            IServiceScopeFactory scopeFactory,
            ILogger<ReplenishmentBackgroundService> logger,
            IConfiguration configuration)
        {
            _scopeFactory = scopeFactory;
            _logger = logger;

            var minutes = configuration.GetValue<int>("ReplenishmentJob:IntervalMinutes", 60);
            _interval = TimeSpan.FromMinutes(minutes);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation(
                "Replenishment background job started. Interval: {Interval}", _interval);

            // Run once at startup, then on the configured interval
            while (!stoppingToken.IsCancellationRequested)
            {
                await RunCheckAsync();
                await Task.Delay(_interval, stoppingToken);
            }
        }

        private async Task RunCheckAsync()
        {
            try
            {
                using var scope = _scopeFactory.CreateScope();
                var service = scope.ServiceProvider.GetRequiredService<IReplenishmentService>();
                var result = await service.RunReplenishmentCheckAsync();

                _logger.LogInformation(
                    "Replenishment check complete. New requests created: {Count}",
                    result.NewRequestsCreated);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error running scheduled replenishment check");
            }
        }
    }
}
