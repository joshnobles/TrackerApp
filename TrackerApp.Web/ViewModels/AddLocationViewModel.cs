using System.ComponentModel.DataAnnotations;

namespace TrackerApp.Web.ViewModels
{
    public class AddLocationViewModel
    {
        [Required]
        [Range(-90, 90)]
        public double Latitude { get; set; }

        [Required]
        [Range(-180, 180)]
        public double Longitude { get; set; }

        [Required]
        [Range(-420, 30000)]
        public double Altitude { get; set; }

        [Required]
        [Range(0.1, 99.99)]
        public double Confidence { get; set; }
    }
}
