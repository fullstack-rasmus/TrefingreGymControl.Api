using FastEndpoints;
using TrefingreGymControl.Api.Domain.Exceptions;
using TrefingreGymControl.Api.Domain.Users;

namespace TrefingreGymControl.Features.Admin.Users.AddUser;

sealed class Endpoint : Endpoint<Request, Response, Mapper>
{
    private IUserService _userService;
    public Endpoint(IUserService userService)
    {
        _userService = userService;
    }
    public override void Configure()
    {
        Post("/admin/users");
        Policies("AdminOrAbove");
    }

    public override async Task HandleAsync(Request req, CancellationToken ct)
    {
        try
        {
            await _userService.RegisterUserAsync(req.Fullname, req.Email, req.Password, req.Role);
        }
        catch (EmailAlreadyInUseException ex)
        {
            AddError(ex.Message);
            await SendErrorsAsync(cancellation: ct);
            return;
        }

        await SendOkAsync(ct);
    }
}