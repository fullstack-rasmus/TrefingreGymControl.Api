using FastEndpoints;
using TrefingreGymControl.Api.Domain.Receipts;

namespace TrefingreGymControl.Features.Users.GetReceipts;

sealed class Endpoint : Endpoint<Request, Response, Mapper>
{
    private readonly IReceiptService _receiptService;
    public Endpoint(IReceiptService receiptService)
    {
        _receiptService = receiptService;
    }
    public override void Configure()
    {
        Get("/users/{userid}/receipts");
        Policies("SelfOnly", "UserOrAbove");
    }

    public override async Task HandleAsync(Request req, CancellationToken ct)
    {
        await SendAsync(Map.FromEntity(await _receiptService.GetReceiptsByUserIdAsync(req.UserId, ct)), cancellation: ct);
    }
}