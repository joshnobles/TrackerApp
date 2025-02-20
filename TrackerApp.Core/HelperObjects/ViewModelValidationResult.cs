using System.ComponentModel.DataAnnotations;

namespace TrackerApp.Web.ViewModels
{
    public class ViewModelValidationResult
    {
        public bool IsValid { get; set; }

        public List<ValidationResult> ErrorResults { get; set; }

        public ViewModelValidationResult()
        {
            IsValid = false;
            ErrorResults = [];
        }
    }
}
