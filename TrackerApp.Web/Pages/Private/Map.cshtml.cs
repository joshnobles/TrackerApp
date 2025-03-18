using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using TrackerApp.Core.DataAccess;
using TrackerApp.Core.Models;
using TrackerApp.Web.Logging;

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

            var userID = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrWhiteSpace(userID))
            {
                await _log.ErrorAsync("Unable to identify authenticated user");
                return Redirect("/Index");
            }

            var currentUser = new User()
            {
                UserID = userID,
                Name = User.Identity.Name,
                Email = User.FindFirstValue(ClaimTypes.Email),
                ProfileImageSrc = User.FindFirstValue("picture")
            };

            if (!await UserExistsAsync(_context, currentUser))
            {
                await CreateUserAsync(_context, currentUser);
                await _log.InformationAsync($"New user created: {{ id: {currentUser.UserID}, name: {currentUser.Name ?? "NULL"}, email: {currentUser.Email ?? "NULL"} }}");
            }
            else
                await _log.InformationAsync($"User logged in: {{ id: {currentUser.UserID}, name: {currentUser.Name ?? "NULL"}, email: {currentUser.Email ?? "NULL"} }}");

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

        private static Task<bool> UserExistsAsync(Context context, User user) =>
            context.User.AnyAsync(u => u.UserID.Equals(user.UserID));

        private static async Task CreateUserAsync(Context context, User user)
        {
            await context.User.AddAsync(user);
            await context.SaveChangesAsync();
        }

    }
}
