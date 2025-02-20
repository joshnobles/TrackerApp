using System.ComponentModel.DataAnnotations;

namespace TrackerApp.Web.ViewModels
{
    public class AddLocationViewModel
    {
        [Required]
        public double Latitude { get; set; }

        [Required]
        public double Longitude { get; set; }

        [Required]
        public double Altitude { get; set; }

        [Required]
        public double Confidence { get; set; }

        [Required]
        public string RequestVerificationSecret { get; set; } = null!;
    }
}
