using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using TrackerApp.Core.DataAccess;
using TrackerApp.Core.Services.Static;
using TrackerApp.Web.Logging;
using TrackerApp.Web.ViewModels;

namespace TrackerApp.Web.Pages.Private
{
    public class AdminModel : PageModel
    {
        private readonly Context _context;
        private readonly Log<AdminModel> _log;

        public AdminModel(Context context, ILogger<AdminModel> logger, IWebHostEnvironment environment)
        {
            _context = context;
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

        public async Task<IActionResult> OnGetAllUsers()
        {
            var users = await _context.User
                .ToArrayAsync();

            return new JsonResult(users);
        }

        public async Task<IActionResult> OnGetSpecificUser([FromQuery] string userID)
        {
            if (string.IsNullOrWhiteSpace(userID))
                return BadRequest();

            var user = await _context.User
                .FirstOrDefaultAsync(x => EF.Functions.Like(x.UserID, userID));

            return user is null ? NotFound() : new JsonResult(user);
        }

        public async Task<IActionResult> OnPostEditUser([FromBody] EditUserViewModel viewModel)
        {
            if (string.IsNullOrWhiteSpace(viewModel.UserID))
                return BadRequest(Array.Empty<int>());

            var validationResult = Valid.ViewModel(viewModel);

            if (!validationResult.IsValid)
                return BadRequest(validationResult.ErrorResults);

            var editedUser = await _context.User
                .FirstOrDefaultAsync(x => EF.Functions.Like(x.UserID, viewModel.UserID));

            if (editedUser is null)
                return NotFound();

            editedUser.Name = viewModel.Name?.Trim();
            editedUser.Email = viewModel.Email?.Trim();
            editedUser.ProfileImageSrc = viewModel.ProfileImageSrc?.Trim();

            await _context.SaveChangesAsync();

            await _log.InformationAsync($"User: {editedUser.UserID} was updated");

            return new EmptyResult();
        }
    
        public async Task<IActionResult> OnPostDeleteUser([FromBody] string userID)
        {
            if (string.IsNullOrWhiteSpace(userID))
                return BadRequest();

            var userToDelete = await _context.User
                .FirstOrDefaultAsync(x => EF.Functions.Like(x.UserID, userID));

            if (userToDelete is null)
                return NotFound();

            _context.User
                .Remove(userToDelete);

            await _context.SaveChangesAsync();

            await _log.InformationAsync($"User: {userID} was deleted");

            return new EmptyResult();
        }

        public async Task<IActionResult> OnGetAllLocations()
        {
            var locations = await _context.Location
                .ToListAsync();

            return new JsonResult(locations);
        }

        public async Task<IActionResult> OnPostDeleteLocations()
        {
            _context.Location
                .RemoveRange(_context.Location);

            await _context.SaveChangesAsync();

            return new EmptyResult();
        }

    }
}
