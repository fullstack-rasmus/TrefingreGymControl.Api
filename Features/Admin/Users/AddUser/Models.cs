using System.Data;
using FastEndpoints;
using FluentValidation;

namespace TrefingreGymControl.Features.Admin.Users.AddUser;

sealed class Request
{
    public string Fullname { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string Role { get; set; }
}

sealed class Validator : Validator<Request>
{
    public Validator()
    {
    }
}

sealed class Response
{
    public string Message => "This endpoint hasn't been implemented yet!";
}
