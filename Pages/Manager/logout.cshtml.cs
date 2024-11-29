using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace weekday.Pages.Manager
{
    public class logout : PageModel
    {
        private readonly ILogger<logout> _logger;

        public logout(ILogger<logout> logger)
        {
            _logger = logger;
        }

        public async Task<IActionResult> OnGet()
        {
            return RedirectToPage("../Login");
        }
    }
}