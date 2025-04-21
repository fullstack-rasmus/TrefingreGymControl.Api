using FastEndpoints;
using TrefingreGymControl.Api.Domain.Fees.Dto;

namespace TrefingreGymControl.Features.Admin.Fees.GetFees;

sealed class Response
{
    public List<FeeDto> Fees { get; set; }
}
