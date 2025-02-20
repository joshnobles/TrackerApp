using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TrackerApp.Core.Models
{
    [Table("Location")]
    public class Location
    {
        [Key]
        public int Id { get; set; }

        [Column("Date_Recorded")]
        public DateTime DateRecorded { get; set; }

        public double Latitude { get; set; }

        public double Longitude { get; set; }

        public double Altitude { get; set; }

        public double Confidence { get; set; }
    }
}
