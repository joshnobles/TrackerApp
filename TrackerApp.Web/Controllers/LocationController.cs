using Microsoft.AspNetCore.Mvc;
using TrackerApp.Core.DataAccess;
using TrackerApp.Core.Models;
using TrackerApp.Core.Services.Interfaces;
using TrackerApp.Core.Services.Static;
using TrackerApp.Web.ViewModels;

namespace TrackerApp.Web.Controllers
{
    [Route("api/[controller]/")]
    [ApiController]
    public class LocationController : ControllerBase
    {
        private readonly Context _context;
        private readonly ISecretService _secretService;

        public LocationController(Context context, ISecretService secretService)
        {
            _context = context;
            _secretService = secretService;
        }

        [HttpPost("AddLocation")]
        public async Task<IActionResult> AddLocation([FromBody] AddLocationViewModel viewModel)
        {
            var validationResults = Valid.ViewModel(viewModel);

            if (!validationResults.IsValid)
                return new JsonResult(validationResults.ErrorResults);

            var requestVerificationSecret = _secretService.GetSecret();

            if (!requestVerificationSecret.Equals(viewModel.RequestVerificationSecret))
                return Unauthorized();

            var newLocation = new Location()
            {
                DateRecorded = DateTime.Now,
                Latitude = viewModel.Latitude,
                Longitude = viewModel.Longitude,
                Altitude = viewModel.Altitude,
                Confidence = viewModel.Confidence
            };

            await _context.Location
                .AddAsync(newLocation);

            await _context.SaveChangesAsync();

            return Created();
        }
    }
}
