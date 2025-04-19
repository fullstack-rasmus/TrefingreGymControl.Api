using TrefingreGymControl.Domain.Subscriptions.Dto;

namespace TrefingreGymControl.Api.Domain.Receipts.Dto;
public class ReceiptDto
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid UserId { get; set; }
    public Guid SubscriptionId { get; set; }
    public SubscriptionDto Subscription { get; set; }
    public string Description { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public ReceiptType ReceiptType { get; set; }
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
}
