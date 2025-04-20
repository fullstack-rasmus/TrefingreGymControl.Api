using FastEndpoints;
using TrefingreGymControl.Api.Domain.Payments;
using TrefingreGymControl.Api.Domain.Payments.Dto;

namespace TrefingreGymControl.Features.Payments.GetPayment;

sealed class Mapper : Mapper<Request, Response, Payment>
{
    public override Response FromEntity(Payment payment)
    {
        if (payment is SubscriptionPayment subscriptionPayment)
        {
            return new Response
            {
                Payment = new SubscriptionPaymentDto
                {
                    Id = payment.Id,
                    Amount = payment.Amount,
                    CreatedAt = payment.CreatedAt,
                    PaymentProviderSessionId = payment.PaymentProviderSessionId,
                    Status = payment.Status,
                    SubscriptionTypeId = subscriptionPayment.SubscriptionTypeId
                }
            };
        }
        else
            return null;
    }
}