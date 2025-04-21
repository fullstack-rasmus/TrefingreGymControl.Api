using FastEndpoints;

namespace TrefingreGymControl.Features.Admin.Fees.DeleteFee;

sealed class Request
{
    [BindFrom("feeId")]
    public Guid FeeId { get; set; }
}

sealed class Validator : Validator<Request>
{
    public Validator()
    {
        
    }
}

sealed class Response
{
    public string Message => "Fee deleted successfully";
}
