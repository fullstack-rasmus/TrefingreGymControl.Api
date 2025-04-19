using FastEndpoints;
using FluentValidation;

namespace TrefingreGymControl.Features.Users.DeleteUser;

sealed class Request
{
    [BindFrom("userId")]
    public Guid UserId { get; set; }
}

sealed class Validator : Validator<Request>
{
    public Validator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty()
            .WithMessage("User ID is required.")
            .Must(x => Guid.TryParse(x.ToString(), out _))
            .WithMessage("Invalid User ID format.");   
    }
}

sealed class Response
{
    public string Message => "User deleted successfully.";
}
