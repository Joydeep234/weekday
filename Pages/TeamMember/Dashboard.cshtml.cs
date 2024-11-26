using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

using Microsoft.Extensions.Logging;


using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using weekday.Data.Context;
using weekday.Data.Entity;
using weekday.Models.Team_Member;

namespace weekday.Pages.TeamMember
{
    public class Dashboard : PageModel
    {
        private readonly AppDbcontext _context;
        

        private readonly ILogger<Dashboard> _logger;

        public Dashboard(ILogger<Dashboard> logger,AppDbcontext context)
        {
            _logger = logger;
            _context = context;
        }
        [BindProperty]
        public TaskMdl TaskIP {  get; set; }

        /*public List<ProjectTask> PTList { get; set; }= new List<ProjectTask>(); *///

        public List<ProjectTask> PTList { get; set; } = new List<ProjectTask>();
        public int employeeID;
        public void OnGet(int empId = 2)
        {
            employeeID = empId;
            
            PTList = _context.projecttask.ToList();

            /*var validID = _context.projecttask.FirstOrDefaultAsync();*/


        }


    }
}