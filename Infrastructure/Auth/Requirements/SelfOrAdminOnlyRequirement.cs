using Microsoft.AspNetCore.Authorization;

namespace TrefingreGymControl.Api.Infrastructure.Auth.Requirements
{
    public class SelfOrAdminOnlyRequirement : IAuthorizationRequirement { }
    public class SelfOrAdminOnlyHandler : AuthorizationHandler<SelfOrAdminOnlyRequirement>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public SelfOrAdminOnlyHandler(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext ctx, SelfOrAdminOnlyRequirement requirement)
        {
            var httpContext = _httpContextAccessor.HttpContext;
            var userId = ctx.User.FindFirst("userId")?.Value;
            var isAdmin = ctx.User.IsInRole("Admin");

            var routeUserId = httpContext?.Request.RouteValues["userId"]?.ToString();

            if (isAdmin || userId == routeUserId)
                ctx.Succeed(requirement);

            return Task.CompletedTask;
        }
    }
}