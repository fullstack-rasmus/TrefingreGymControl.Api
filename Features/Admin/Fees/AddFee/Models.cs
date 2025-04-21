using FastEndpoints;

namespace TrefingreGymControl.Features.Admin.Fees.AddFee;

sealed class Request
{
    public string Description { get; set; }
    public decimal Amount { get; set; }
    public bool IsRecurring { get; set; }
}

sealed class Validator : Validator<Request>
{
    public Validator()
    {
        
    }
}

sealed class Response
{
    public string Message => "Fee added successfully.";
}
