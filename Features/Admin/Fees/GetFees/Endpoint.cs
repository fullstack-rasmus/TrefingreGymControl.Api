using FastEndpoints;
using TrefingreGymControl.Api.Domain.Fees;

namespace TrefingreGymControl.Features.Admin.Fees.GetFees;

sealed class Endpoint : EndpointWithoutRequest<Response, Mapper>
{
    private readonly IFeeService _feesService;

    public Endpoint(IFeeService feesService)
    {
        _feesService = feesService;
    }
    public override void Configure()
    {
        Get("/admin/fees");
        Policies("AdminOrAbove");
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        await SendAsync(Map.FromEntity(await _feesService.GetFeesAsync(ct)), cancellation: ct);
    }
}