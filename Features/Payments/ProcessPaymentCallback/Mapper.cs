using FastEndpoints;
using TrefingreGymControl.Api.Domain.Payments;
using TrefingreGymControl.Api.Domain.Subscriptions;
using TrefingreGymControl.Domain.Subscriptions.Dto;

namespace TrefingreGymControl.Features.Payments.ProcessPaymentCallback;

sealed class Mapper : Mapper<Request, Response, Subscription>
{
    public override Response FromEntity(Subscription e)
    {
        return new Response
        {
            Subscription = new SubscriptionDto
            {
                Id = e.Id,
                SubscriptionTypeId = e.SubscriptionTypeId,
                StartDate = e.StartDate,
                EndDate = e.EndDate,
                CreatedAt = e.CreatedAt,
                IsActive = e.IsActive,
                UserId = e.UserId,
                IsCanceled = e.IsCanceled,
                Price = e.Price,
                SubscriptionType = new SubscriptionTypeDto
                {
                    Id = e.SubscriptionType.Id,
                    Name = e.SubscriptionType.Name,
                }
            }
        };
    }
}