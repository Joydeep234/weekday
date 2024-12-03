using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace weekday.Pages
{
    public class Pricing : PageModel
    {
        private readonly ILogger<Pricing> _logger;

        public Pricing(ILogger<Pricing> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
        }
    }
}