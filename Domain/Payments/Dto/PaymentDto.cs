using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TrefingreGymControl.Api.Domain.Payments.Dto
{
    public class PaymentDto
    {
        public Guid Id { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public decimal Amount { get; set; }
        public string PaymentProviderSessionId { get; set; }
        public PaymentStatus Status { get; set; }
    }
}