namespace TrefingreGymControl.Api.Domain.Subscriptions
{
    public interface ISubscriptionTypeService
    {
        Task DeleteSubscriptionTypeAsync(Guid subscriptionTypeId, CancellationToken cancellationToken = default);
    }
}