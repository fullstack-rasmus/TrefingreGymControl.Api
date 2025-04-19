using FastEndpoints;
using FluentValidation;

namespace TrefingreGymControl.Features.Subscriptions.BuySubscription;

sealed class Request
{
    [BindFrom("userId")]
    public Guid UserId { get; set; }
    [BindFrom("subscriptionTypeId")]
    public Guid SubscriptionTypeId { get; set; }
    public DateTimeOffset StartSubscriptionAt { get; set; }
}

sealed class Validator : Validator<Request>
{
    public Validator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("UserId is required.");
        RuleFor(x => x.SubscriptionTypeId)
            .NotEmpty().WithMessage("SubscriptionTypeId is required.");
        RuleFor(x => x.StartSubscriptionAt)
            .NotEmpty().WithMessage("StartSubscriptionAt is required.")
            .Must(x => x.Date >= DateTimeOffset.UtcNow.Date).WithMessage("StartSubscriptionAt must be today or in the future.");
    }
}

sealed class Response
{
    public string Message => "This endpoint hasn't been implemented yet!";
}
