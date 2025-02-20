using System.ComponentModel.DataAnnotations;
using TrackerApp.Web.ViewModels;

namespace TrackerApp.Core.Services.Static
{
    public static class Valid
    {
        public static ViewModelValidationResult ViewModel(object viewModel)
        {
            var result = new ViewModelValidationResult();

            var vc = new ValidationContext(viewModel);

            if (!Validator.TryValidateObject(viewModel, vc, result.ErrorResults, true))
                result.IsValid = false;
            else
                result.IsValid = true;

            return result;
        }
    }
}
