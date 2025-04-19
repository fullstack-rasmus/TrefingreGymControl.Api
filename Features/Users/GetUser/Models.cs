using FastEndpoints;
using FluentValidation;

namespace TrefingreGymControl.Features.Users.GetUser;

sealed class Request
{
    public Guid UserId { get; set; }
}

sealed class Validator : Validator<Request>
{
    public Validator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty()
            .WithMessage("User ID is required.");
    }
}

sealed class Response
{
    public string Message => "User retrieved successfully.";
    public string Fullname { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string ProfilePictureUrl { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
    public List<string> Notifications { get; set; } = new();
}
