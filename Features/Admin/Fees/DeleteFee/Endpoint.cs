using FastEndpoints;
using TrefingreGymControl.Api.Domain.Fees;

namespace TrefingreGymControl.Features.Admin.Fees.DeleteFee;

sealed class Endpoint : Endpoint<Request, Response, Mapper>
{
    private readonly IFeeService _feeService;
    public Endpoint(IFeeService feeService)
    {
        _feeService = feeService;
    }

    public override void Configure()
    {
        Delete("/admin/fees/{feeId}");
        Policies("AdminOrAbove");
    }

    public override async Task HandleAsync(Request req, CancellationToken ct)
    {
        await _feeService.DeleteFeeAsync(req.FeeId, ct);
        await SendNoContentAsync(ct);
    }
}