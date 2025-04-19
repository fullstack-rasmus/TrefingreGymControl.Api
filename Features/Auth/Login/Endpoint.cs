using FastEndpoints;
using TrefingreGymControl.Api.Domain.Users;

namespace TrefingreGymControl.Features.Auth.Login;

sealed class Endpoint : Endpoint<Request, Response, Mapper>
{
    private readonly IUserService _userService;
    public Endpoint(IUserService userService)
    {
        this._userService = userService;
    }

    public override void Configure()
    {
        Post("auth/login");
        AllowAnonymous();
    }

    public override async Task HandleAsync(Request req, CancellationToken ct)
    {
        var token = await _userService.AuthenticateUserAsync(req.Email, req.Password, ct);
        Response.Token = token;
        await SendOkAsync(Response, ct);
    }
}