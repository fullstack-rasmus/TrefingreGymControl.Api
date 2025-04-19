namespace TrefingreGymControl.Api.Domain.Subscriptions
{
    public class SubscriptionType
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; }
        public decimal Price { get; set; }
        public SubscriptionDurationUnit SubscriptionDurationUnit { get; set; }
        public int DurationValue { get; set; }
        public bool IsActive { get; set; } = true;
        public bool IsRecurring { get; set; }

        public SubscriptionType(string name, decimal price, SubscriptionDurationUnit subscriptionDurationUnit, int durationValue)
        {
            Name = name;
            Price = price;
            SubscriptionDurationUnit = subscriptionDurationUnit;
            DurationValue = durationValue;
        }

        public void Deactivate()
        {
            IsActive = false;
        }
        
        public void Activate()
        {
            IsActive = true;
        }

        public void Update(string name, decimal price)
        {
            Name = name;
            Price = price;
        }

        public void MakeRecurring()
        {
            IsRecurring = true;
        }
        
        public void MakeNonRecurring()
        {
            IsRecurring = false;
        }
    }
}