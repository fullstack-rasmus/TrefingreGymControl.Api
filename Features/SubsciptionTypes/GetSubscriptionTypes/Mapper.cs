using FastEndpoints;
using TrefingreGymControl.Api.Domain.Subscriptions;
using TrefingreGymControl.Domain.Subscriptions.Dto;

namespace TrefingreGymControl.Features.SubscriptionTypes.GetSubscriptionTypes;

sealed class Mapper : ResponseMapper<Response, object>
{
    public override Response FromEntity(object e)
    {
        var subscriptionTypes = e as List<SubscriptionType>;
        if (subscriptionTypes == null)
            throw new ArgumentNullException(nameof(e), "Expected a list of SubscriptionType.");

        return new Response
        {
            SubscriptionTypes = subscriptionTypes.Select(st => new SubscriptionTypeDto
            {
                Id = st.Id,
                Name = st.Name,
                DurationValue = st.DurationValue,
                DurationUnit = st.SubscriptionDurationUnit,
                Price = st.Price,
                IsActive = st.IsActive,
                IsRecurring = st.IsRecurring,
                IsDeleted = st.IsDeleted,
                Resources = st.AccessibleResources.Select(r => new ResourceDto
                {
                    Id = r.Id,
                    Name = r.Name
                }).ToList()
            }).ToList()
        };
    }

}