using CourseWork.Utility;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace CourseWork.WebApp.Controllers
{
    
    public class AccountController : Controller
    {
        public IActionResult Login(string returnUrl = "/")
        {
            if (!Url.IsLocalUrl(returnUrl))
                returnUrl = "/";

            TempData["ReturnUrl"] = returnUrl;

            return Challenge(new AuthenticationProperties
            {
                RedirectUri = Url.Action(nameof(LoginCallback))
            });
        }

        public IActionResult LoginCallback()
        {
            var returnUrl = TempData["ReturnUrl"] as string;

            if (string.IsNullOrEmpty(returnUrl))
            {
                returnUrl = Url.Action(nameof(HomeController.Index), "Home");
            }

            return Redirect(returnUrl);
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction(nameof(HomeController.Index), "Home");
        }
    }
}
