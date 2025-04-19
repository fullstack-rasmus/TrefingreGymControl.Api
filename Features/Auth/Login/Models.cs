using FastEndpoints;

namespace TrefingreGymControl.Features.Auth.Login;

sealed class Request
{
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}

sealed class Validator : Validator<Request>
{
    public Validator()
    {
        
    }
}

sealed class Response
{
    public string Message => "User logged in successfully.";
    public string Token { get; set; } = string.Empty;
}
