using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;
using TrackerApp.Web.Logging;

namespace TrackerApp.Web.Pages.Private
{
    [Authorize(Policy = "IsAdmin")]
    public class AdminModel : PageModel
    {
        private readonly Log<AdminModel> _log;

        public AdminModel(ILogger<AdminModel> logger, IWebHostEnvironment environment)
        {
            _log = new Log<AdminModel>(logger, environment);
        }

        public async Task<IActionResult> OnGet()
        {
            var userID = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrWhiteSpace(userID))
            {
                await _log.ErrorAsync("An unidentified user accessed Admin page");
                return Redirect("/Index");
            }

            await _log.InformationAsync($"User: {userID} accessed Admin page");

            return Page();
        }
    }
}
