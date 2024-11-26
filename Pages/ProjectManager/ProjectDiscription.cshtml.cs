using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Runtime.CompilerServices;
using weekday.Data.Context;
using weekday.Data.Entity;
using weekday.Models;
using weekday.Pages.HR;

namespace weekday.Pages.ProjectManager
{
    public class ProjectDiscriptionModel : PageModel
    {
        public readonly AppDbcontext _context;

        public ProjectDiscriptionModel(AppDbcontext context)
        {
            _context = context;
        }

        [BindProperty]
        public ProjectDisplay ProjectDis { get; set; } = new ProjectDisplay();

        public List<Employee> Employees { get; set; } = new List<Employee>();

       

        [BindProperty]
        public AddTeamLead addTeamLead { get; set; }

        [BindProperty]
        public List<TeamList> teamList { get; set; } = new List<TeamList>();
        [BindProperty]
        public FormData FormData { get; set; } = new FormData();

        public StatusUpdate statusUpdate { get; set; } = new StatusUpdate();

        public async Task OnGetAsync(int ProjectId)
        {
            TempData["tempProjectId"] = ProjectId;

            ProjectDis = await (from p in _context.project
                                where p.ProjectId == ProjectId
                                select new ProjectDisplay
                                {
                                    ProjectId = p.ProjectId,
                                    ProjectName = p.Name,
                                    About = p.About,
                                    ProjectStatus = p.Status,
                                    StartDate = p.StartDate,
                                    EndDate = p.EndDate,
                                    Deadline = p.Deadline,
                                    Project_Details = p.Details,
                                }).FirstOrDefaultAsync() ?? new ProjectDisplay();



            Employees = await (from Employee in _context.employee
                               join Designation in _context.designation
                               on Employee.DesignationId equals Designation.DesignationId
                               join teamMembers in _context.teamMembers on Employee.EmployeeId equals teamMembers.MemberId
                               where Designation.DesignationId == 3 & teamMembers.status == "Active"
                               select new Employee
                               {
                                   EmployeeId = Employee.EmployeeId,
                                   FirstName = Employee.FirstName,
                                   LastName = Employee.LastName,
                                   Email = Employee.Email,
                                   Password = Employee.Password,
                                   Status = Employee.Status,
                                   ImageURL = Employee.ImageURL,
                                   DesignationId = Employee.DesignationId,
                                   OrgId = Employee.OrgId
                               }).ToListAsync();

            teamList = await (from team in _context.team
                              where team.ProjectId == ProjectId
                              select new TeamList
                              {
                                  TeamId = team.TeamId,
                                  ProjectId = team.ProjectId,
                                  TeamName = team.Name,
                                  TeamStatus = team.Status
                              }).ToListAsync() ?? new List<TeamList>();
        }


        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> OnPostCreateAsync([FromBody] FormData formData)
        {
            bool team_exist = await _context.teamMembers.AnyAsync(x => x.TeamId == formData.teamId);
            //var teamMember = await _context.teamMembers.FindAsync(formData.teamId);

            if (!team_exist)
            {

                TeamMembers teamMembers = new TeamMembers
                {
                    TeamId = formData.teamId,
                    MemberId = formData.teamLeaderId,
                    DesignationId = 3,
                    OrgId = 1,
                    status = "InActive"
                };
                
                await _context.AddAsync(teamMembers);
                await _context.SaveChangesAsync();
                return new JsonResult(new { success = true });
            }
            else
            {
                Console.WriteLine("Team already Exists");
                return new JsonResult(new { success = false });
            }

            

            
        }
        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> OnPostUpdateStatusAsync(string status)
        {
            Console.WriteLine(status);

            int proj_ID = Convert.ToInt32(TempData["tempProjectId"]);

            var project_id = await _context.project.FindAsync(proj_ID);

            project_id.Status = status;

            await _context.SaveChangesAsync();




            return new JsonResult(new { success = true });
        }
 

        
    }

    public class FormData()
    {
        public int teamId { get; set; }
        public int teamLeaderId { get; set; }
    }

    public class StatusUpdate()
    {
        public string status { get; set; }
    }
}
