using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using weekday.Data.Context;
using weekday.Data.Entity;
using weekday.Models;

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
        public ProjectDisplay ProjectDis {  get; set; } = new ProjectDisplay();

        public List<Employee> Employees { get; set; }

        [BindProperty]
        public AddTeamLead addTeamLead { get; set; }

        [BindProperty]
        public List<TeamList> teamList { get; set; } = new List<TeamList>();
      
        public async Task OnGetAsync(int ProjectId)
        {
         
                ProjectDis =  (from p in _context.project
                              join team in _context.team on p.ProjectId equals team.ProjectId
                              join teamMember in _context.teamMembers on team.TeamId equals teamMember.TeamId
                              join Employee in _context.employee on teamMember.MemberId equals Employee.EmployeeId
                              where p.ProjectId ==  ProjectId
                              select new ProjectDisplay
                              {
                                  ProjectId = p.ProjectId,
                                  ProjectName = p.Name,
                                  About = p.About,
                                  ProjectStatus = p.Status,
                                  StartDate = p.StartDate,
                                  EndDate = p.EndDate,
                                  Deadline = p.Deadline,
                                  Details = p.Details,
                                  TeamId = team.TeamId,
                                  TeamName = team.Name,
                                  Description = team.Description,
                                  ImageURL = Employee.ImageURL, 
                              }).FirstOrDefault();
 


            Employees = await (from Employee in _context.employee
                         join Designation in _context.designation
                         on Employee.DesignationId equals Designation.DesignationId
                         where Designation.DesignationId == 3
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
                              select new TeamList { 
                                  TeamId = team.TeamId,
                                  ProjectId=team.ProjectId,
                                  TeamName = team.Name,
                                  TeamStatus = team.Status
                              }).ToListAsync();
        }
        public JsonResult OnGetEmployees(int employeeId)
        {
            Console.WriteLine($"Employees: {employeeId}");
            var emp_name = _context.employee.Find(employeeId);
            var employee_Name = emp_name.FirstName + " " +emp_name.LastName;

            return new JsonResult(new { emp_name });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult OnPostCreateOrUpdate([FromBody] AddTeamLead team)
        {
            
            Console.WriteLine(team.IdList.Count);
            //Console.WriteLine(HttpContext.Request.Body);
            //Console.WriteLine(TLarray.TeamLeadId);
            Console.WriteLine($"MembersId: {string.Join(", ", team.IdList)}");

            // Debugging output to check received data
            /*Console.WriteLine($"ProjectId: {team.ProjectId}");
            Console.WriteLine($"ProjectName: {team.ProjectName}");
            */

            // Save to the database

            return new JsonResult(new { success = true });
        }
    }
}
