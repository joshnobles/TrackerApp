using Microsoft.AspNetCore.Authorization;

namespace TrackerApp.Web.Policies.Requirements
{
    public class IsLoggedInRequirement : IAuthorizationRequirement
    {
        public IsLoggedInRequirement()
        {
            
        }
    }
}
