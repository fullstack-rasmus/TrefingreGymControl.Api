using Microsoft.EntityFrameworkCore;
using TrefingreGymControl.Api.Persistence;

namespace TrefingreGymControl.Api.BackgroundServices.Subscriptions
{
    public class SubscriptionCancellationBackgroundService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<SubscriptionCancellationBackgroundService> _logger;

        public SubscriptionCancellationBackgroundService(IServiceProvider serviceProvider, ILogger<SubscriptionCancellationBackgroundService> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    _logger.LogInformation("SubscriptionCancellationBackgroundService is running.");
                    using var scope = _serviceProvider.CreateScope();
                    var db = scope.ServiceProvider.GetRequiredService<TFGymControlDbContext>();

                    var expiredSubs = await db.Subscriptions
                        .Where(s => s.IsCanceled && s.IsActive && s.EndDate < DateTimeOffset.UtcNow)
                        .ToListAsync(stoppingToken);
                    
                    _logger.LogInformation("Found {Count} expired subscriptions to process", expiredSubs.Count);

                    foreach (var sub in expiredSubs)
                    {
                        sub.DeactivateIfExpired();
                        _logger.LogInformation("Deactivated canceled subscription {Id} due to Enddate being reached", sub.Id);
                    }
                    
                    await db.SaveChangesAsync(stoppingToken);

                    _logger.LogInformation("Processed {Count} expired subscriptions", expiredSubs.Count);
                    _logger.LogInformation("SubscriptionCancellationBackgroundService completed processing.");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "SubscriptionCancellationBackgroundService, An error occurred while processing subscriptions.");
                }

                await Task.Delay(TimeSpan.FromHours(24), stoppingToken);
            }
        }
    }
}