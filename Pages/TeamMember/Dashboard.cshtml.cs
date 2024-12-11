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
using Microsoft.AspNetCore.Authorization;

namespace weekday.Pages.TeamMember
{
    [Authorize (Policy= "TEAM_MEMBERS")]
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
        public int ProjectId;
        public string ProjectName;
        //prjid and prjName getting from ProjectDashboard page
        public async Task OnGetAsync(int prjid, string prjName, int empId = 1) //assigned parameters follows un-assigned parameters 
        {// empId need to get from session after implemention login page 
            employeeID = empId;
            ProjectName = prjName;
            ProjectId = prjid;

            PTList = _context.projecttask.ToList();

            /*PTList=await (from ProjectTask in _context.projecttask where 
                
                )*/





            /*var validID = _context.projecttask.FirstOrDefaultAsync();*/


        }


    }
}