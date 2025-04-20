using FastEndpoints;

namespace TrefingreGymControl.Features.Admin.Resource.AddResources;

sealed class Request
{
    public string Name { get; set; } = default!;
}

sealed class Validator : Validator<Request>
{
    public Validator()
    {
        
    }
}

sealed class Response
{
    public string Message => "Resource created successfully";
}
