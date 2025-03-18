using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace TrackerApp.Web.Pages
{
    public class CallbackModel : PageModel
    {
        public IActionResult OnGet()
        {
            return Redirect("/Private/Map");
        }
    }
}
