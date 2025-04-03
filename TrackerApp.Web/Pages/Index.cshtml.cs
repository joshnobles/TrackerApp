using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using TrackerApp.Core.DataAccess;
using TrackerApp.Core.Models;
using TrackerApp.Core.Services.Static;
using TrackerApp.Web.Logging;
using TrackerApp.Web.Pages.Private;
using TrackerApp.Web.ViewModels;

namespace TrackerApp.Web.Pages
{
    public class IndexModel : PageModel
    {
        private readonly Context _context;
        private readonly Log<IndexModel> _log;

        public IndexModel(Context context, IWebHostEnvironment environment, ILogger<IndexModel> logger)
        {
            _context = context;

            _log = new(logger, environment);
        }

        public async Task<IActionResult> OnGet() 
        {
            if (User.Identity is not null && User.Identity.IsAuthenticated)
            {
                var actionResult = await VerifyUserInDatabase(User, _context, _log);
                return actionResult;
            }

            return Page();
        }

        private static async Task<IActionResult> VerifyUserInDatabase(ClaimsPrincipal user, Context context, Log<IndexModel> log)
        {
            var userID = user.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrWhiteSpace(userID))
            {
                await log.ErrorAsync("Unable to identify authenticated user");
                return new RedirectResult("/Account/Logout");
            }

            var viewModel = new AddUserViewModel
            {
                UserID = userID,
                Name = user.Identity!.Name,
                Email = user.FindFirstValue(ClaimTypes.Email),
                ProfileImageSrc = user.FindFirstValue("picture")
            };

            var validationResult = Valid.ViewModel(viewModel);

            if (!validationResult.IsValid)
            {
                var errMsg = "Authenticated user containted invalid data: ";

                foreach (var err in validationResult.ErrorResults)
                    errMsg += err.ErrorMessage + " ";

                await log.ErrorAsync(errMsg);

                return new RedirectResult("/Account/Logout");
            }

            var currentUser = new User
            {
                UserID = viewModel.UserID,
                Name = viewModel.Name,
                Email = viewModel.Email,
                ProfileImageSrc = viewModel.ProfileImageSrc
            };

            if (!await UserExistsAsync(context, currentUser))
            {
                await CreateUserAsync(context, currentUser);
                await log.InformationAsync($"New user created: {{ id: {currentUser.UserID}, name: {currentUser.Name ?? "NULL"}, email: {currentUser.Email ?? "NULL"} }}");
            }
            else
                await log.InformationAsync($"User logged in: {{ id: {currentUser.UserID}, name: {currentUser.Name ?? "NULL"}, email: {currentUser.Email ?? "NULL"} }}");

            return new RedirectResult("/Private/Map");
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
