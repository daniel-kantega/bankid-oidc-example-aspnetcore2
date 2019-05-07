using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace BankIdAspNetCore2Demo.Controllers
{
    public class AccountController : Controller
    {
        //
        // GET: /Account/SignIn
        [HttpGet]
        public IActionResult SignIn()
        {
            var authProperties = new AuthenticationProperties { RedirectUri = "/" };

            // The redirect to OpenID Connect server has its own session. 
            // Put session parameters into the AuthenticationProperties to be picked up by our event handler.
            new List<string> {
                "login_hint",
                "ui_locales",
            }
            .ForEach(key => {
                    string value = HttpContext.Session?.GetString(key);
                    if (!string.IsNullOrEmpty(value))
                    {
                        authProperties.Items.Add(key, value);
                    }
                });

            var result = Challenge(authProperties, OpenIdConnectDefaults.AuthenticationScheme);
            return result;
        }

        //
        // GET: /Account/SignOut
        [HttpGet]
        public async System.Threading.Tasks.Task<IActionResult> SignOut()
        {
            await AuthenticationHttpContextExtensions.SignOutAsync(HttpContext);

            return RedirectToAction("Index", "Home");
        }


        //
        // GET: /Account/AccessDenied
        [AllowAnonymous]
        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
