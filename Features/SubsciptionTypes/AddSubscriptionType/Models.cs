﻿using System.Data;
using FastEndpoints;
using FluentValidation;
using TrefingreGymControl.Api.Domain.Fees.Dto;
using TrefingreGymControl.Api.Domain.Subscriptions;
using TrefingreGymControl.Domain.Subscriptions.Dto;

namespace TrefingreGymControl.Features.SubscriptionTypes.AddSubscriptionType;

sealed class Request
{
    public string Name { get; set; }
    public int DurationValue { get; set; }
    public SubscriptionDurationUnit DurationUnit { get; set; }
    public decimal Price { get; set; }
    public bool IsRecurring { get; set; }
    public List<ResourceDto> Resources { get; set; } = new();
    public List<FeeDto> Fees { get; set; } = new();
}

sealed class Validator : Validator<Request>
{
    public Validator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Name is required.");
        RuleFor(x => x.DurationValue)
            .GreaterThan(0)
            .WithMessage("Duration value must be greater than 0.");
        RuleFor(x => x.DurationUnit)
            .IsInEnum()
            .WithMessage("Invalid duration unit.");
        RuleFor(x => x.Price)
            .GreaterThan(0)
            .WithMessage("Price must be greater than 0.");
    }
}

sealed class Response
{
    public string Message => "Subscription type added successfully.";
}
