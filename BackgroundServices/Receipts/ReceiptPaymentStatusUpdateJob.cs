using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrefingreGymControl.Api.Domain.Payments;
using TrefingreGymControl.Api.Domain.Receipts;
using TrefingreGymControl.Api.Persistence;

namespace TrefingreGymControl.Api.BackgroundServices.Receipts
{
    public class ReceiptPaymentStatusUpdateJob : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<ReceiptPaymentStatusUpdateJob> _logger;

        public ReceiptPaymentStatusUpdateJob(IServiceProvider serviceProvider, ILogger<ReceiptPaymentStatusUpdateJob> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("ReceiptPaymentStatusUpdateJob is running.");
                using var scope = _serviceProvider.CreateScope();
                var dbContext = scope.ServiceProvider.GetRequiredService<TFGymControlDbContext>();
                var receiptService = scope.ServiceProvider.GetRequiredService<IReceiptService>();
                var paymentService = scope.ServiceProvider.GetRequiredService<IPaymentService>();

                var receiptsPendingPayment = await receiptService.GetAllPendingReceiptsAsync(stoppingToken);
                var reciptsUpdated = 0;
                foreach (var receipt in receiptsPendingPayment)
                {
                    var payment = await paymentService.GetPaymentFromSubscription(receipt.SubscriptionId, stoppingToken);
                    if (payment != null)
                    {
                        if (payment.Status == PaymentStatus.Completed)
                        {
                            receipt.Paid();
                            _logger.LogInformation("Receipt {receiptId} payment status updated to completed.", receipt.Id);
                            reciptsUpdated++;
                        }
                    }
                }

                await dbContext.SaveChangesAsync(stoppingToken);
                _logger.LogInformation("Total receipts updated: {reciptsUpdated}", reciptsUpdated);

                _logger.LogInformation("ReceiptPaymentStatusUpdateJob completed processing.");
                await Task.Delay(TimeSpan.FromHours(24), stoppingToken);
            }
        }
    }
}