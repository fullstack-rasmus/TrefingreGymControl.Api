using TrefingreGymControl.Api.Domain.Fees.Dto;
using TrefingreGymControl.Api.Domain.Subscriptions;

namespace TrefingreGymControl.Domain.Subscriptions.Dto;

public class SubscriptionTypeDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int DurationValue { get; set; }
    public SubscriptionDurationUnit DurationUnit { get; set; }
    public decimal Price { get; set; }
    public bool IsActive { get; set; }
    public bool IsRecurring { get; set; }
    public bool IsDeleted { get; set; }
    public List<ResourceDto> Resources { get; set; } = new List<ResourceDto>();
    public List<FeeDto> Fees { get; set; } = new List<FeeDto>();
}
