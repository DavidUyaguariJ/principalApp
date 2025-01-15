using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace HumanTalentApp.Pages.Users
{
    [Authorize(Roles = "ADMN")]
    public class CreateModel : PageModel
    {
        public void OnGet()
        {
        }
    }
}
