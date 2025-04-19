namespace TrefingreGymControl.Api.Domain.Exceptions
{
    public class SubscriptionAlreadyExistsException : LoggedException
    {
        public SubscriptionAlreadyExistsException(Guid userId, Guid subscriptionTypeId, ILogger logger) : base($"User {userId} already has an active subscription of type {subscriptionTypeId}.", logger)
        {
        }
    }
}