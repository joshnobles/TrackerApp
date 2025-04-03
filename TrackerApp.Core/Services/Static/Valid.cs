using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using TrackerApp.Web.ViewModels;

namespace TrackerApp.Core.Services.Static
{
    public static class Valid
    {
        public static ViewModelValidationResult ViewModel(object viewModel)
        {
            var result = new ViewModelValidationResult();

            var vc = new ValidationContext(viewModel);
            
            result.IsValid = Validator.TryValidateObject(viewModel, vc, result.ErrorResults, true);

            return result;
        }
    
        public static bool Role(string role)
        {
            return Regex.IsMatch(role, @"^[A-Za-z]{3,20}$");
        }
    }
}
