namespace TrefingreGymControl.Api.Domain.Exceptions
{
    public class SubscriptionNotFoundException : LoggedException
    {
        public SubscriptionNotFoundException(string subscriptionId, ILogger logger) : base($"Subscription with id {subscriptionId} not found.", logger)
        {
        }
    }
}