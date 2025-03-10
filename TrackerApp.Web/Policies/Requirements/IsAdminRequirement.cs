using Microsoft.AspNetCore.Authorization;

namespace TrackerApp.Web.Policies.Requirements
{
    public class IsAdminRequirement : IAuthorizationRequirement
    {
        public string IsAdmin { get; }

        public IsAdminRequirement(string isAdmin)
        {
            IsAdmin = isAdmin;
        }
    }
}
