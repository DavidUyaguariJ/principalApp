using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HumanTalentApp.Areas.Identity.Controllers
{
    [Area("Identity")]
    [Route("[area]/[controller]/{action=Index}")]
    [Authorize(Roles = "ADMN")]
    public class UsersController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Users()
        {
            return View();
        }
    }
}
