using FastEndpoints;
using TrefingreGymControl.Api.Domain.Fees.Dto;
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
                Status = r.Status,
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
                        IsDeleted = r.Subscription.SubscriptionType.IsDeleted,
                        Fees = r.Subscription.SubscriptionType.Fees.Select(f => new FeeDto
                        {
                            Id = f.Id,
                            Description = f.Description,
                            Amount = f.Amount,
                            IsRecurring = f.IsRecurringFee
                        }).ToList(),
                        IsRecurring = r.Subscription.SubscriptionType.IsRecurring
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