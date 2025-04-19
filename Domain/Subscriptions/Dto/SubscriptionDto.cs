using TrefingreGymControl.Domain.Subscriptions.Dto;

namespace TrefingreGymControl.Domain.Subscriptions.Dto;

public class SubscriptionDto
{
    public Guid Id { get; set; }
    public Guid SubscriptionTypeId { get; set; }
    public Guid UserId { get; set; }
    public DateTimeOffset StartDate { get; set; }
    public DateTimeOffset EndDate { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public decimal Price { get; set; }
    public bool IsActive { get; set; }
    public bool IsCanceled { get; set; }
    public SubscriptionTypeDto SubscriptionType { get; set; } = new();
}