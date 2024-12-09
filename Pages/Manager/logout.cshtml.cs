using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace weekday.Pages.Manager
{
    [Authorize]
    public class logout : PageModel
    {
        private readonly ILogger<logout> _logger;

        public logout(ILogger<logout> logger)
        {
            _logger = logger;
        }

        public async Task<IActionResult> OnGet()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToPage("../Index");
        }
    }
}