using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using TrackerApp.Core.DataAccess;
using TrackerApp.Core.Models;
using TrackerApp.Core.Services.Static;
using TrackerApp.Web.Logging;
using TrackerApp.Web.ViewModels;

namespace TrackerApp.Web.Pages.Private
{
    public class MapModel : PageModel
    {
        private readonly Context _context;
        private readonly Log<MapModel> _log;

        public MapModel(Context context, IWebHostEnvironment environment, ILogger<MapModel> logger)
        {
            _context = context;

            _log = new(logger, environment);
        }

        public async Task<IActionResult> OnGet()
        {
            if (User.Identity is null || !User.Identity.IsAuthenticated)
            {
                await _log.ErrorAsync("Unauthenticated user accessed map page");
                return Redirect("/Index");
            }

            return Page();
        }

        public async Task<IActionResult> OnGetUserProfile()
        {
            if (User.Identity is null || !User.Identity.IsAuthenticated)
            {
                await _log.ErrorAsync("Unauthenticated user accessed map page");
                return Redirect("/Index");
            }

            var userID = User.FindFirst(c => c.Type.Equals(ClaimTypes.NameIdentifier))?.Value;

            if (string.IsNullOrWhiteSpace(userID))
            {
                await _log.ErrorAsync("Unable to identify authenticated user");
                return Redirect("/Index");
            }

            var user = await _context.User
                .FirstOrDefaultAsync(u => u.UserID.Equals(userID));

            if (user is null)
            {
                await _log.ErrorAsync($"Authenticated user not found in database: {{ userID: {userID} }}");
                return NotFound();
            }

            return new JsonResult(user);
        }

        public async Task<IActionResult> OnGetNextLocation()
        {
            var location = await _context.Location
                .OrderBy(l => l.DateRecorded)
                .LastOrDefaultAsync();

            if (location is null)
                return NotFound();

            return new JsonResult(location);
        }
    }
}
