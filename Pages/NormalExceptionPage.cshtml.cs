using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace weekday.Pages
{
    public class NormalExceptionPage : PageModel
    {
        private readonly ILogger<NormalExceptionPage> _logger;

        public NormalExceptionPage(ILogger<NormalExceptionPage> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
        }
    }
}