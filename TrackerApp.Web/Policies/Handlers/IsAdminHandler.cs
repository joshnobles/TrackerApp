using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using TrackerApp.Core.Services.Interfaces;
using TrackerApp.Web.Policies.Requirements;

namespace TrackerApp.Web.Policies.Handlers
{
    public class IsAdminHandler : AuthorizationHandler<IsAdminRequirement>
    {
        private readonly IAuthenticationManagementApi _authManagementApi;

        public IsAdminHandler(IAuthenticationManagementApi authManagementApi)
        {
            _authManagementApi = authManagementApi;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, IsAdminRequirement requirement)
        {
            var userID = context.User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrWhiteSpace(userID))
            {
                context.Fail();
                return;
            }

            var roles = await _authManagementApi.GetUserRolesAsync(userID);

            if (roles.Any(r => r.Equals("Admin")))
            {
                context.Succeed(requirement);
                return;
            }

            context.Fail();
        }
    }
}
