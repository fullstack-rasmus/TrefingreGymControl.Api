
using Microsoft.EntityFrameworkCore;
using TrefingreGymControl.Api.Domain.Exceptions;
using TrefingreGymControl.Api.Persistence;
using TrefingreGymControl.Api.Utils;

namespace TrefingreGymControl.Api.Domain.Subscriptions
{
    public class SubscriptionService : ISubscriptionService
    {
        private readonly ILogger<SubscriptionService> _logger;
        private readonly TFGymControlDbContext _dbContext;

        public SubscriptionService(TFGymControlDbContext dbContext, ILogger<SubscriptionService> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task AddSubscriptionTypeAsync(SubscriptionType subscriptionType, CancellationToken ct = default)
        {
            var existingSubscriptionType = await _dbContext.SubscriptionTypes
                .FirstOrDefaultAsync(x => x.Name == subscriptionType.Name, ct);
            if (existingSubscriptionType != null)
                throw new SubscriptionTypeAlreadyExistsException($"Subscription type with name {subscriptionType.Name} already exists.", _logger);

            subscriptionType.Deactivate();
            _dbContext.SubscriptionTypes.Add(subscriptionType);
            _logger.LogInformation("Adding subscription type: {SubscriptionType}", subscriptionType);
            await _dbContext.SaveChangesAsync(ct);
        }

        public async Task<Subscription> BuySubscriptionAsync(Guid userId, Guid subscriptionTypeId, DateTimeOffset startDate, CancellationToken ct = default)
        {
            var subscriptionType = _dbContext.SubscriptionTypes
                .FirstOrDefault(x => x.Id == subscriptionTypeId) ?? throw new SubscriptionTypeNotFoundException(subscriptionTypeId.ToString(), _logger);

            var user = _dbContext.Users
                .FirstOrDefault(x => x.Id == userId) ?? throw new NoUserWithIdFoundException(userId.ToString(), _logger);

            var existingSubscription = _dbContext.Subscriptions
                .FirstOrDefault(x => x.UserId == userId && x.SubscriptionTypeId == subscriptionTypeId && x.IsActive);

            if (existingSubscription != null && existingSubscription.IsDeleted == false)
                throw new SubscriptionAlreadyExistsException(userId, subscriptionTypeId, _logger);

            Subscription subscription = Subscribe(userId, startDate, subscriptionType);

            await _dbContext.Subscriptions.AddAsync(subscription);
            _logger.LogInformation("Buying subscription: {Subscription}", subscription);
            return subscription;
        }

        public Subscription Subscribe(Guid userId, DateTimeOffset startDate, SubscriptionType subscriptionType, bool isResubscription = false)
        {
            var subscription = Subscription.RegisterNewSubscription(userId, subscriptionType.Id, subscriptionType.Price, isResubscription);

            _logger.LogInformation("Registering new subscription subscription: {SubscriptionId}", subscription.Id);
            subscription.SetSubscriptionType(subscriptionType.Id);
            _logger.LogInformation("Subscription: {SubscriptionId} Setting subscription type: {SubscriptionType}", subscription.Id, subscriptionType);
            subscription.SetStartDate(startDate);
            _logger.LogInformation("Subscription: {SubscriptionId} Setting start date: {StartDate}", subscription.Id, startDate);
            subscription.SetEndDate(CalculateExpirationDate(subscriptionType, startDate));
            _logger.LogInformation("Subscription: {SubscriptionId} Setting end date: {EndDate}", subscription.Id, subscription.EndDate);
            subscription.SetUser(userId);
            _logger.LogInformation("Subscription: {SubscriptionId} Setting user: {UserId}", subscription.Id, userId);
            subscription.SetPrice(subscriptionType.Price);
            _logger.LogInformation("Subscription: {SubscriptionId} Setting price: {Price}", subscription.Id, subscription.Price);
            subscription.Activate(isResubscription);
            _logger.LogInformation("Subscription: {SubscriptionId} Activating subscription", subscription.Id);

            return subscription;
        }

        public DateTimeOffset CalculateExpirationDate(SubscriptionType subscriptionType, DateTimeOffset? fromDate = null)
        {
            var start = fromDate ?? DateTimeOffset.UtcNow;
            DateTimeOffset expiration;
            expiration = DomainMath.EndDateCalculator(subscriptionType, start);

            _logger.LogInformation("Calculated expiration date: {ExpirationDate} for subscription type: {SubscriptionType}", expiration, subscriptionType);
            return expiration;
        }

        public Task CancelSubscriptionAsync(Guid subscriptionId, CancellationToken ct = default)
        {
            var subscription = _dbContext.Subscriptions
                .FirstOrDefault(x => x.Id == subscriptionId) ?? throw new SubscriptionNotFoundException(subscriptionId.ToString(), _logger);

            subscription.Cancel();
            _logger.LogInformation("Subscription: {SubscriptionId} Canceled", subscription.Id);
            return _dbContext.SaveChangesAsync(ct);
        }

        public async Task<List<SubscriptionType>> GetSubscriptionTypesAsync(bool onlyActive = false, CancellationToken ct = default)
        {
            if (onlyActive)
            {
                var activeSubscriptionTypes = await _dbContext.SubscriptionTypes
                    .Include(x => x.AccessibleResources)
                    .Include(x=> x.Fees)
                    .Where(x => x.IsActive && !x.IsDeleted)
                    .ToListAsync(ct);
                return activeSubscriptionTypes;
            }
            else
            {
                var allSubscriptionTypes = await _dbContext.SubscriptionTypes
                    .Include(x => x.AccessibleResources)
                    .Include(x=> x.Fees)
                    .ToListAsync(ct);
                return allSubscriptionTypes;
            }
        }

        public async Task ToogleSubscriptionTypeActivationStateAsync(Guid subscriptionTypeId, bool isActive, CancellationToken ct = default)
        {
            var subscriptionType = await _dbContext.SubscriptionTypes
                .FirstOrDefaultAsync(x => x.Id == subscriptionTypeId, ct) ?? throw new SubscriptionTypeNotFoundException(subscriptionTypeId.ToString(), _logger);

            if (isActive)
                subscriptionType.Activate();
            else
                subscriptionType.Deactivate();

            _logger.LogInformation("Subscription type: {SubscriptionTypeId} activated", subscriptionTypeId);
            await _dbContext.SaveChangesAsync(ct);
        }

        public async Task<List<Subscription>> GetUserSubscriptionsAsync(Guid userId, CancellationToken ct = default)
        {
            return await _dbContext.Subscriptions
                .Include(x => x.SubscriptionType)
                .Where(x => x.UserId == userId)
                .ToListAsync(ct);
        }

        public async Task<Subscription> GetSubscriptionAsync(Guid subscriptionId, CancellationToken ct = default)
        {
            var subscription = await _dbContext.Subscriptions
                .Include(x => x.SubscriptionType)
                .FirstOrDefaultAsync(x => x.Id == subscriptionId) ?? throw new SubscriptionNotFoundException(subscriptionId.ToString(), _logger);

            return subscription;
        }
    }
}