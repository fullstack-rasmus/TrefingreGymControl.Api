using FastEndpoints;
using TrefingreGymControl.Api.Domain.Fees;

namespace TrefingreGymControl.Features.Admin.Fees.AddFee;

sealed class Endpoint : Endpoint<Request, Response, Mapper>
{
    private readonly IFeeService _feeService;

    public Endpoint(IFeeService feeService)
    {
        _feeService = feeService;
    }

    public override void Configure()
    {
        Post("/admin/fees");
        Policies("AdminOrAbove");
    }

    public override async Task HandleAsync(Request req, CancellationToken ct)
    {
        await _feeService.CreateFeeAsync(req.Description, req.Amount, req.IsRecurring, ct);
        await SendAsync(Response, cancellation: ct);
    }
}