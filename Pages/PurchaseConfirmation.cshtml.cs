using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using weekday.Data.Context;
using weekday.Middleware;

namespace weekday.Pages
{
    [Authorize(Policy = "NoDesignationOrManager")]
    public class PurchaseConfirmation : PageModel
    {
        private readonly AppDbcontext _context;
        private readonly ILogger<PurchaseConfirmation> _logger;

        public PurchaseConfirmation(ILogger<PurchaseConfirmation> logger,AppDbcontext dbcontext)
        {
            _logger = logger;
            _context = dbcontext;
        }

        public int planID { get; set; }

        public async Task<IActionResult> OnGet(int planid)
        {
            planID = planid;
             var userid = User.FindFirst("empID")?.Value;
            if(userid==null)return RedirectToPage("/Login");
            return Page();
        }

        
    }
}