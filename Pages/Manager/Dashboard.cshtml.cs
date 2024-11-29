using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using weekday.Data.Context;
using weekday.Data.Entity;

namespace weekday.Pages.Manager
{
    public class Dashboard : PageModel
    {
        private readonly AppDbcontext _context;
        private readonly ILogger<Dashboard> _logger;

        public Dashboard(ILogger<Dashboard> logger,AppDbcontext dbcontext)
        {
            _logger = logger;
            _context = dbcontext;
        }

        public List<Project> projectlist {get;set;} = new List<Project>();
        // public List<Employee> emp {get;set;} = new List<Employee>();    
        public int totalproject {get;set;}
        public int inprogress {get;set;}    
        public int completeproject {get;set;}
        public int totalemployees {get;set;}
        public async Task OnGet()
        {
            try
            {
                projectlist = await _context.project.Where(p=>p.OrgId==2).ToListAsync();
                foreach(var pr in projectlist){
                    totalproject++;
                    if(pr.Status=="InProgress")inprogress++;
                    if(pr.Status=="Completed")completeproject++;
                }
                
                totalemployees = await _context.employee.Where(o=>o.OrgId==2).CountAsync();
            }
            catch (System.Exception e)
            {
                System.Console.WriteLine($"dashboard error=>{e.Message}");
            }
        }
    }
}