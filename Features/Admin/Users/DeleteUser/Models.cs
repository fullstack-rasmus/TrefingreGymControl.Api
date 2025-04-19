using FastEndpoints;
using FluentValidation;

namespace TrefingreGymControl.Features.Admin.Users.DeleteUser;

sealed class Request
{
    [BindFrom("userId")]
    public Guid UserId { get; set; }
}

sealed class Validator : Validator<Request>
{
    public Validator()
    {
        RuleFor(r => r.UserId)
            .NotEmpty()
            .WithMessage("User ID is required.");        
    }
}

sealed class Response
{
    public string Message => "User deleted successfully.";
}
