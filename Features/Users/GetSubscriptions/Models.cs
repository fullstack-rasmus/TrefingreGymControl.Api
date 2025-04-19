using System.Data;
using FastEndpoints;
using FluentValidation;
using TrefingreGymControl.Domain.Subscriptions.Dto;
using TrefingreGymControl.Features.SubscriptionTypes.GetSubscriptionTypes;

namespace TrefingreGymControl.Features.Users.GetSubscriptions;

sealed class Request
{
    [BindFrom("userId")]
    public Guid UserId { get; set; }
}

sealed class Validator : Validator<Request>
{
    public Validator()
    {
        RuleFor(r => r.UserId)
            .NotEmpty()
            .WithMessage("User ID is required.");
    }
}

sealed class Response
{
    public List<SubscriptionDto> Subscriptions { get; set; } = new();
}
