using FastEndpoints;
using FluentValidation;

namespace TrefingreGymControl.Features.Users.UpdateUser;

sealed class Request
{
    [FromClaim("userId")]
    public Guid UserId { get; set; }
    public string? Fullname { get; set; }
}

sealed class Validator : Validator<Request>
{
    public Validator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty()
            .WithMessage("UserId is required");
        RuleFor(x => x.Fullname)
            .NotEmpty()
            .WithMessage("Fullname is required");
    }
}

sealed class Response
{
    public string Message => "User updated successfully.";
}
