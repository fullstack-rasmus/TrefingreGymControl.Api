using Microsoft.AspNetCore.Authorization;

namespace TrefingreGymControl.Api.Infrastructure.Auth.Permissions
{
    public class SelfOnlyRequirement : IAuthorizationRequirement { }
    public class SelfOnlyHandler : AuthorizationHandler<SelfOnlyRequirement>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public SelfOnlyHandler(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, SelfOnlyRequirement requirement)
        {

            var httpContext = _httpContextAccessor.HttpContext;
            var routeId = httpContext?.Request.RouteValues["userId"]?.ToString();
            var userId = context.User.FindFirst("UserId")?.Value;

            if (!string.IsNullOrEmpty(routeId) && routeId == userId)
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}