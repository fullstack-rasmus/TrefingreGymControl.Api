using FastEndpoints;
using TrefingreGymControl.Api.Domain.Receipts;
using TrefingreGymControl.Api.Domain.Receipts.Dto;
using TrefingreGymControl.Domain.Subscriptions.Dto;

namespace TrefingreGymControl.Features.Users.GetReceipts;

sealed class Mapper : Mapper<Request, Response, List<Receipt>>
{
    public override Response FromEntity(List<Receipt> receipts)
    {
        return new Response
        {
            Receipts = receipts.Select(r => new ReceiptDto
            {
                Id = r.Id,
                UserId = r.UserId,
                SubscriptionId = r.SubscriptionId,
                Description = r.Description,
                ReceiptType = r.ReceiptType,
                Subscription = new SubscriptionDto
                {
                    Id = r.Subscription.Id,
                    SubscriptionType = new SubscriptionTypeDto
                    {
                        Id = r.Subscription.SubscriptionType.Id,
                        Name = r.Subscription.SubscriptionType.Name,
                        Price = r.Subscription.SubscriptionType.Price,
                        DurationUnit = r.Subscription.SubscriptionType.SubscriptionDurationUnit,
                        DurationValue = r.Subscription.SubscriptionType.DurationValue,
                        IsActive = r.Subscription.SubscriptionType.IsActive,
                    },
                    StartDate = r.Subscription.StartDate,
                    EndDate = r.Subscription.EndDate,
                    IsActive = r.Subscription.IsActive,
                    CreatedAt = r.Subscription.CreatedAt,
                    IsCanceled = r.Subscription.IsCanceled,
                    Price = r.Subscription.Price,
                    UserId = r.Subscription.UserId
                },
                Price = r.Price,
                CreatedAt = r.CreatedAt
            }).ToList()
        };
    }

}