using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using TrefingreGymControl.Api.Domain.Notifications;
using TrefingreGymControl.Api.Domain.Subscriptions.Events;
using TrefingreGymControl.Api.Persistence;

namespace TrefingreGymControl.Api.Application.Subscriptions
{
    public class SubscriptionTypeDeletedEventHandler : IEventHandler<SubscriptionTypeDeletedEvent>
    {
        private readonly TFGymControlDbContext _dbContext;
        private readonly ILogger<SubscriptionTypeDeletedEventHandler> _logger;
        private readonly INotificationService _notificationService;
        public SubscriptionTypeDeletedEventHandler(TFGymControlDbContext dbContext, ILogger<SubscriptionTypeDeletedEventHandler> logger, INotificationService notificationService)
        {
            _notificationService = notificationService;
            _logger = logger;
            _dbContext = dbContext;
        }
        public async Task HandleAsync(SubscriptionTypeDeletedEvent deleteEvent, CancellationToken ct)
        {
            var subscriptions = await _dbContext.Subscriptions
                .Include(x => x.SubscriptionType)
                .Where(x => x.SubscriptionTypeId == deleteEvent.SubscriptionTypeId)
                .ToListAsync();

            foreach (var subscription in subscriptions)
            {
                try
                {
                    subscription.Cancel();
                    await _notificationService.AddNotificationAsync(subscription.UserId, $"Your subscription {subscription.SubscriptionType.Name} has been cancelled.", ct);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error cancelling subscription {SubscriptionId} for user {UserId}", subscription.Id, subscription.UserId);
                }
            }
        }
    }
}