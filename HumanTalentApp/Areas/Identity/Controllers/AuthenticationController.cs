using HumanTalentApp.Areas.Identity.Models;
using HumanTalentApp.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HumanTalentApp.Areas.Identity.Controllers
{
    [Area("Identity")]
    [Route("[area]/[controller]/{action=Index}")]
    [AllowAnonymous]
    public class AuthenticationController : Controller
    {
        private readonly AuthService _authService;
        public AuthenticationController(AuthService authService)
        {
            _authService = authService;
        }
        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> Login(LoginViewModel loginModel)
        {
            if (loginModel.Password != null && loginModel.UserName != null)
            {
                if (ModelState.IsValid)
                {
                    var success = await _authService.Login(loginModel);
                    if (!string.IsNullOrEmpty(success))
                    {
                        HttpContext.Session.SetString("Token", success);
                        return LocalRedirect("~/");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Credenciales incorrectas");
                        return View("Login");

                    }
                }
            }
            return View("Login");
        }
    }
}
