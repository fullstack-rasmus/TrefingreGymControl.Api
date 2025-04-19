namespace TrefingreGymControl.Api.Domain.Exceptions
{
    public class SubscriptionTypeNotFoundException : LoggedException
    {
        public SubscriptionTypeNotFoundException(string subscriptionTypeId, ILogger logger) : base($"Subscription type with id {subscriptionTypeId} not found.", logger)
        {
        }
    }
}