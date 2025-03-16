using System.ComponentModel.DataAnnotations;

namespace TrackerApp.Web.ViewModels
{
    public class EditUserViewModel
    {
        [Required]
        public string UserID { get; set; } = null!;

        private string? _name;
        private string? _email;
        private string? _profileImageSrc;

        [StringLength(50)]
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
        [Display(Name = "Profile Image Source")]
        public string? ProfileImageSrc
        {
            get { return _profileImageSrc; }
            set { _profileImageSrc = string.IsNullOrWhiteSpace(value) ? null : value; }
        }
    }
}
