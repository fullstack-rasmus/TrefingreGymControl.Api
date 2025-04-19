namespace TrefingreGymControl.Api.Domain.Exceptions
{
    public class SubscriptionTypeAlreadyExistsException : LoggedException
    {
        public SubscriptionTypeAlreadyExistsException(string name, ILogger logger) : base($"Subscription type '{name}' already exists.", logger)
        {
        }
    }
}