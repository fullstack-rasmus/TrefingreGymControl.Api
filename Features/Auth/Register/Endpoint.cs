using FastEndpoints;
using TrefingreGymControl.Api.Domain.Exceptions;
using TrefingreGymControl.Api.Domain.Users;

namespace TrefingreGymControl.Features.Auth.Register;

sealed class Endpoint : Endpoint<Request, Response, Mapper>
{
    private readonly IUserService _userService;
    public Endpoint(IUserService userService)
    {
        this._userService = userService;
    }
    public override void Configure()
    {
        Post("auth/register");
        AllowAnonymous();
    }

    public override async Task HandleAsync(Request req, CancellationToken ct)
    {
        try
        {
            try
            {
                await _userService.RegisterUserAsync(req.Fullname, req.Email, req.Password);
            }
            catch (EmailAlreadyInUseException ex)
            {
                AddError(ex.Message);
                await SendErrorsAsync(cancellation: ct);
                return;
            }

            await SendOkAsync(ct);
        }
        catch (Exception ex)
        {
            AddError(ex.Message);
            await SendErrorsAsync(cancellation: ct);
        }
    }
}