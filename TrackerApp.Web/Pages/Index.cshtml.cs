using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace TrackerApp.Web.Pages
{
    public class IndexModel : PageModel
    {
        public IActionResult OnGet() 
        {
            if (User.Identity is not null && User.Identity.IsAuthenticated)
                return Redirect("/Private/Map");

            return Page();
        }
    }
}
