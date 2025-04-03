using System.ComponentModel.DataAnnotations;

namespace TrackerApp.Web.ViewModels
{
    public class AddUserViewModel
    {
        private string? _name;
        private string? _email;
        private string? _profileImageSrc;

        [Required]
        [RegularExpression(@"^[A-Za-z0-9|\.\-]{3,250}$")]
        public string UserID { get; set; } = null!;

        [RegularExpression("^[A-Za-z '-]{3,50}$")]
        public string? Name
        {
            get { return _name; }
            set { _name = string.IsNullOrWhiteSpace(value) ? null : value; }
        }

        [StringLength(50)]
        [EmailAddress]
        public string? Email
        {
            get { return _email; }
            set { _email = string.IsNullOrWhiteSpace(value) ? null : value; }
        }

        [StringLength(250)]
        [RegularExpression("^https://.+")]
        [Display(Name = "Profile Image Source")]
        public string? ProfileImageSrc
        {
            get { return _profileImageSrc; }
            set { _profileImageSrc = string.IsNullOrWhiteSpace(value) ? null : value; }
        }
    }
}
