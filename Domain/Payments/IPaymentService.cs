using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TrefingreGymControl.Api.Domain.Payments
{
    public interface IPaymentService
    {
        Task<Payment> CreatePaymentAsync(Guid subscriptionTypeId, Guid userId, CancellationToken cancellationToken = default);
        Task<Payment> GetPaymentAsync(Guid paymentId, CancellationToken cancellationToken = default);
        Task<Payment?> GetPaymentFromSubscription(Guid subscriptionId, CancellationToken cancellationToken = default);
    }
}