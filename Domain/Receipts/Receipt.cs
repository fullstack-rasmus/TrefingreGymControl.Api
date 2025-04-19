using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrefingreGymControl.Api.Domain.Subscriptions;

namespace TrefingreGymControl.Api.Domain.Receipts
{
    public class Receipt
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid UserId { get; set; }
        public Guid SubscriptionId { get; set; }
        public Subscription Subscription { get; set; }
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public ReceiptType ReceiptType { get; set; }
        public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;

        public Receipt(ReceiptType receiptType)
        {
            ReceiptType = receiptType;
        }

        public static Receipt NewReceipt(ReceiptType receiptType)
        {
            return new Receipt(receiptType);
        }

        public void AttachToUser(Guid userId)
        {
            UserId = userId;
        }
        public void AttachToSubscription(Subscription subscription)
        {
            Subscription = subscription;
        }

        public void SetPrice(decimal price)
        {
            Price = price;
        }
        
        public void SetDescription(string description)
        {
            Description = description;
        }
    }
}