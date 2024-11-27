using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace weekday.Pages.Manager
{
    public class AllTeamShow : PageModel
    {
        private readonly ILogger<AllTeamShow> _logger;

        public AllTeamShow(ILogger<AllTeamShow> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
        }
    }
}