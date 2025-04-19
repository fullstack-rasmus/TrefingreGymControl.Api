using FastEndpoints;
using TrefingreGymControl.Api.Domain.Receipts.Dto;

namespace TrefingreGymControl.Features.Users.GetReceipts;

sealed class Request
{
    [BindFrom("userId")]
    public Guid UserId { get; set; }
}

sealed class Validator : Validator<Request>
{
    public Validator()
    {
        
    }
}

sealed class Response
{
    public List<ReceiptDto> Receipts { get; set; }
}
