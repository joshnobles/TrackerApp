using Microsoft.AspNetCore.Authorization;
using TrackerApp.Web.Policies.Requirements;

namespace TrackerApp.Web.Policies.Handlers
{
    public class IsLoggedInHandler : AuthorizationHandler<IsLoggedInRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, IsLoggedInRequirement requirement)
        {
            var isAuthenticated = context.User.Identity is not null && context.User.Identity.IsAuthenticated;

            if (isAuthenticated)
                context.Succeed(requirement);

            return Task.CompletedTask;
        }
    }
}
