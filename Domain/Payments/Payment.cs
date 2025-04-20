using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TrefingreGymControl.Api.Domain.Payments
{
    public abstract class Payment
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid UserId { get; set; }
        public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
        public decimal Amount { get; set; }
        public string PaymentProviderSessionId { get; set; } = default!;
        public PaymentStatus Status { get; set; }
        public string PaymentType { get; set; }

        public Payment(decimal amount, Guid userId)
        {
            UserId = userId;
            Amount = amount;
            Status = PaymentStatus.Pending;
        }

        public void SetSessionId(string id)
        {
            PaymentProviderSessionId = id;
        }
    }

    public class KioskPayment : Payment
    {
        public KioskPayment(decimal amount, Guid userId) : base(amount, userId)
        {
        }

        public string Description { get; set; }
    }
}