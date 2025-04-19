using FastEndpoints;
using TrefingreGymControl.Api.Domain.Subscriptions;
using TrefingreGymControl.Domain.Subscriptions.Dto;
using TrefingreGymControl.Features.SubscriptionTypes.GetSubscriptionTypes;

namespace TrefingreGymControl.Features.Users.GetSubscriptions;

sealed class Mapper : Mapper<Request, Response, object>
{
    public override Response FromEntity(object e)
    {
        var subscriptions = e as List<Subscription>;
        if (subscriptions == null)
            throw new InvalidOperationException("Invalid entity type.");

        return new Response
        {
            Subscriptions = subscriptions.Select(s => new SubscriptionDto
            {
                Id = s.Id,
                SubscriptionTypeId = s.SubscriptionTypeId,
                UserId = s.UserId,
                StartDate = s.StartDate,
                EndDate = s.EndDate,
                CreatedAt = s.CreatedAt,
                Price = s.Price,
                IsActive = s.IsActive,
                IsCanceled = s.IsCanceled,
                SubscriptionType = new SubscriptionTypeDto
                {
                    Id = s.SubscriptionType.Id,
                    Name = s.SubscriptionType.Name,
                    Price = s.SubscriptionType.Price,
                    DurationUnit = s.SubscriptionType.SubscriptionDurationUnit,
                    DurationValue = s.SubscriptionType.DurationValue,
                    IsActive = s.SubscriptionType.IsActive
                }
            }).ToList()
        };
    }
}