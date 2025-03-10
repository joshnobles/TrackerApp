using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;
using TrackerApp.Web.Logging;

namespace TrackerApp.Web.Pages.Account
{
    public class AccessDeniedModel : PageModel
    {
        private readonly Log<AccessDeniedModel> _log;

        public AccessDeniedModel(ILogger<AccessDeniedModel> logger, IWebHostEnvironment environment)
        {
            _log = new Log<AccessDeniedModel>(logger, environment);
        }

        public async Task OnGet()
        {
            var userID = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userID is null)
                await _log.WarningAsync($"An unidentified user was denied access to a page");
            else
                await _log.WarningAsync($"User ID: ${userID} was denied access to a page");
        }
    }
}
