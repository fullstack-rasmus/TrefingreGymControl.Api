namespace TrefingreGymControl.Api.Domain.Subscriptions
{
    public interface ISubscriptionService
    {
        DateTimeOffset CalculateExpirationDate(SubscriptionType subscriptionType, DateTimeOffset? fromDate = null);
        Task AddSubscriptionTypeAsync(SubscriptionType subscriptionType, CancellationToken ct = default);
        Task<List<SubscriptionType>> GetSubscriptionTypesAsync(bool onlyActive = false, CancellationToken ct = default);
        Task BuySubscriptionAsync(Guid userId, Guid subscriptionTypeId, DateTimeOffset startDate, CancellationToken ct = default);
        Task<List<Subscription>> GetUserSubscriptionsAsync(Guid userId, CancellationToken ct = default);
        Task CancelSubscriptionAsync(Guid subscriptionId, CancellationToken ct = default);
        Task ToogleSubscriptionTypeActivationStateAsync(Guid subscriptionTypeId, bool activate, CancellationToken ct = default);
        Subscription Subscribe(Guid userId, DateTimeOffset startDate, SubscriptionType subscriptionType, bool isResubscription = false);
    }
}