using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TrefingreGymControl.Api.Domain.Receipts;
using TrefingreGymControl.Api.Domain.Subscriptions;
using TrefingreGymControl.Api.Persistence;

namespace TrefingreGymControl.Api.BackgroundServices.Subscriptions
{
    public class RecurringSubscriptionJob : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<RecurringSubscriptionJob> _logger;

        public RecurringSubscriptionJob(IServiceProvider serviceProvider, ILogger<RecurringSubscriptionJob> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("RecurringSubscriptionJob is running.");
                using var scope = _serviceProvider.CreateScope();
                var dbContext = scope.ServiceProvider.GetRequiredService<TFGymControlDbContext>();
                var subscriptionService = scope.ServiceProvider.GetRequiredService<ISubscriptionService>();
                var receiptService = scope.ServiceProvider.GetRequiredService<IReceiptService>();

                var now = DateTimeOffset.UtcNow;

                var expired = await dbContext.Subscriptions
                    .Include(s => s.SubscriptionType)
                    .Where(s => s.IsActive && s.SubscriptionType.IsRecurring && s.EndDate < now)
                    .ToListAsync(stoppingToken);

                foreach (var subscription in expired)
                {
                    _logger.LogInformation("Processing subscription with ID {SubscriptionId}", subscription.Id);
                    subscription.IsActive = false;

                    var newSub = subscriptionService.Subscribe(subscription.UserId, now, subscription.SubscriptionType, true);
                    // HVAD MED BETALING? SIGNUP-FEE?
                    await dbContext.Subscriptions.AddAsync(newSub);
                    _logger.LogInformation("Created new subscription with ID {SubscriptionId}", newSub.Id);
                }

                await dbContext.SaveChangesAsync(stoppingToken);
                _logger.LogInformation("RecurringSubscriptionJob completed processing.");
                await Task.Delay(TimeSpan.FromHours(24), stoppingToken);
            }
        }
    }
}