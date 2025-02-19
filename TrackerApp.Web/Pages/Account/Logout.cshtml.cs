using Auth0.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace TrackerApp.Web.Pages.Account
{
    public class LogoutModel : PageModel
    {
        public async Task OnGet()
        {
            var authProperties = new LogoutAuthenticationPropertiesBuilder()
                .WithRedirectUri("/")
                .Build();

            await HttpContext
                .SignOutAsync(Auth0Constants.AuthenticationScheme, authProperties);

            await HttpContext
                .SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        }
    }
}
