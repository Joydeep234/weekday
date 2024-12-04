using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace weekday.Pages
{
    public class AccessDeniedPage : PageModel
    {
        private readonly ILogger<AccessDeniedPage> _logger;

        public AccessDeniedPage(ILogger<AccessDeniedPage> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
        }
    }
}