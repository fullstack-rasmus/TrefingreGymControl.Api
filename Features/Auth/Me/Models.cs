using FastEndpoints;

namespace TrefingreGymControl.Features.Auth.Me;

sealed class Request
{
    [FromClaim("userId")]
    public Guid UserId { get; set; }
}
sealed class Response
{
    public Guid UserId { get; set; }
    public string Fullname { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
}
