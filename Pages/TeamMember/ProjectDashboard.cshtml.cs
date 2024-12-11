using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using weekday.Data.Context;
using weekday.Data.Entity;

namespace weekday.Pages.TeamMember
{
    [Authorize(Policy = "TEAM_MEMBERS")]


    public class ProjectDashboardModel : PageModel
    {
        private readonly AppDbcontext _context;
        //project names based on need to select based on the  
        public ProjectDashboardModel(AppDbcontext context)
        {
            _context = context;
        }
        
        
        public List<ProjectTask> ProjectTaskList { get; set; }=new List<ProjectTask>(); 
        public List<Project> ProjectList { get; set; }=new List<Project>();
        public int EmployeeID {  get; set; }

        

        public async Task OnGetAsync()//int empID=1
        {
            //Retrive employee id from login page
            EmployeeID = int.Parse(User.FindFirst("empID")?.Value);


            ProjectList = await (from ProjectTask in _context.projecttask
                                  join Project in _context.project
                                 on ProjectTask.ProjectId equals Project.ProjectId
                                   where ProjectTask.AssignedForId== EmployeeID
                                 select Project).Distinct().ToListAsync();    
        }

        
    }
}
