using Auth0.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace TrackerApp.Web.Pages.Account
{
    public class LoginModel : PageModel
    {
        public async Task OnGet()
        {
            var authProperties = new LoginAuthenticationPropertiesBuilder()
                .WithRedirectUri("https://trackerstalker.com/Private/Map")
                .Build();

            await HttpContext
                .ChallengeAsync(Auth0Constants.AuthenticationScheme, authProperties);
        }
    }
}
