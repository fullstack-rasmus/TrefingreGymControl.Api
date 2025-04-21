using FastEndpoints;
using TrefingreGymControl.Api.Domain.Fees;
using TrefingreGymControl.Api.Domain.Fees.Dto;

namespace TrefingreGymControl.Features.Admin.Fees.GetFees;

sealed class Mapper : ResponseMapper<Response, List<Fee>>
{
    public override Response FromEntity(List<Fee> e)
    {
        return new Response
        {
            Fees = e.Select(f => new FeeDto
            {
                Id = f.Id,
                Amount = f.Amount,
                Description = f.Description
            }).ToList()
        };
    }

}