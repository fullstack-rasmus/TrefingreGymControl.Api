namespace TrefingreGymControl.Api.Domain.Payments.Dto
{
    public class SubscriptionPaymentDto : PaymentDto
    {
        public Guid SubscriptionTypeId { get; set; }
    }
}