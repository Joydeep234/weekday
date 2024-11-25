using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using weekday.Data.Context;
using weekday.Data.Entity;

namespace weekday.Pages.Manager
{
    public class PastProject : PageModel
    {
        private readonly AppDbcontext _context;
        private readonly ILogger<RecentProject> _logger;

        public PastProject(ILogger<RecentProject> logger,AppDbcontext dbcontext)
        {
            _logger = logger;
            _context = dbcontext;
        }

        public List<Project> AllprojectData {get; set;} = new List<Project>();
        public async Task OnGet()
        {
            try
            {
                AllprojectData = await _context.project.Where(p=>p.OrgId==2 && p.Status=="Completed").ToListAsync();
                if(AllprojectData.Count<1)throw new Exception("Product cannot fetch from db");
            }
            catch (Exception e)
            {
               Console.WriteLine($"products fetching error=>{e.Message}");
            }
        }
    }
}