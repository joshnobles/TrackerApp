using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TrackerApp.Core.Models
{
    [Table("user")]
    public class User
    {
        [Key]
        public string UserID { get; set; } = null!;

        public string? Name { get; set; }

        public string? Email { get; set; }

        public string? ProfileImageSrc { get; set; }
    }
}
