using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using weekday.Data.Context;
using weekday.Data.Entity;
using weekday.Middleware;

namespace weekday.Pages.Manager
{
    public class EmpDetailsClass{
        public string firstname { get; set; }
        public string lastname { get; set;}
        public string status { get; set; }
        public int employeeid { get; set; }
        public bool assigned { get; set; }
    } 
    [Authorize (Policy ="MANAGER")]
    public class AddManagers : PageModel
    {
        private readonly AppDbcontext _context;
        private readonly ILogger<AddManagers> _logger;

        public AddManagers(ILogger<AddManagers> logger, AppDbcontext dbcontext)
        {
            _logger = logger;
            _context = dbcontext;
        }

        // public EmpAndTask empandtask { get; set; }
        [BindProperty]
        public List<int> ManagerList { get; set; }
        public List<Employee> employeelist { get; set; } = new List<Employee>();
        public List<EmpDetailsClass> empdetails { get; set; } = new List<EmpDetailsClass>();
        public ProjectAndTeam projectDetails { get; set; } = new ProjectAndTeam();

        public int projectID { get; set; }
        public int newteamID { get; set; }
        public async Task OnGet(int projID, int teamID)
        {
            projectID = projID;
            newteamID = teamID;
           
                employeelist = await (from e in _context.employee
                                      join d in _context.designation
                                      on e.DesignationId equals d.DesignationId
                                      where d.Name == "PROJECT_MANAGER"
                                      select e).ToListAsync();


                if (employeelist.Count < 1) throw new CustomExceptionClass("Employee list is empty");
                foreach(var emp in employeelist){
                     bool checkalreadyassigned = await (from te in _context.teamMembers
                                   join t in _context.team on te.TeamId equals t.TeamId
                                   join pr in _context.project on t.ProjectId equals pr.ProjectId
                                   where te.MemberId == emp.EmployeeId && pr.ProjectId==projID
                                   select te).AnyAsync();
                    var empdet = new EmpDetailsClass{
                        firstname = emp.FirstName,
                        lastname = emp.LastName,
                        status = emp.Status,
                        employeeid = emp.EmployeeId,
                        assigned = checkalreadyassigned
                    };
                    empdetails.Add(empdet);
                }
               

                projectDetails = await (from p in _context.project
                                        join e in _context.employee
                                        on p.CreatedByID equals e.EmployeeId
                                        join d in _context.designation
                                        on e.DesignationId equals d.DesignationId
                                        where p.ProjectId == projID
                                        select new ProjectAndTeam
                                        {
                                            FirstName = e.FirstName,
                                            LastName = e.LastName,
                                            DesignationName = d.Name,
                                            ProjectId = p.ProjectId,
                                            Name = p.Name,
                                            About = p.About,
                                            CreatedAt = p.CreatedAt,
                                            StartDate = p.StartDate,
                                            EndDate = p.EndDate,
                                            Deadline = p.Deadline,
                                            Details = p.Details,
                                            Status = p.Status
                                        }).FirstOrDefaultAsync();

                if (projectDetails == null) throw new CustomExceptionClass("ProjectDetails is not there");

        }

        public async Task<IActionResult> OnGetEmployeeDetails(int managerId)
        {
            
                var employeedet = await (from e in _context.employee
                                         where e.EmployeeId == managerId && e.Status == "Active"
                                         select new
                                         {
                                             e.FirstName,
                                             e.LastName,
                                             e.Email,
                                             e.EmployeeId
                                         }).FirstOrDefaultAsync();

                System.Console.WriteLine(employeedet.FirstName);

                var empandtask = await (from tmem in _context.teamMembers
                                        join tm in _context.team on tmem.TeamId equals tm.TeamId
                                        join p in _context.project on tm.ProjectId equals p.ProjectId
                                        where tmem.MemberId == managerId && tmem.status == "Active"
                                        select new
                                        {
                                            p.Name
                                        }).ToListAsync();
                return new JsonResult(new
                {
                    success = true,
                    employeedet,
                    empandtask,
                    message = "Successfully MAnager Task Retrived"
                });
            
        }

        public async Task<IActionResult> OnPostAddingMAnagers(int projeID, int postteamID)
        {
           
                if (ManagerList.Count < 1) throw new CustomExceptionClass("Atleast select a member");
                foreach (var i in ManagerList)
                {
                    var desig = await (from e in _context.employee
                                       join d in _context.designation on e.DesignationId equals d.DesignationId
                                       where e.EmployeeId == i
                                       select new
                                       {
                                           d.DesignationId
                                       }).FirstOrDefaultAsync();
                    var teammem = new TeamMembers
                    {
                        TeamId = postteamID,
                        MemberId = i,
                        DesignationId = desig.DesignationId,
                        status = "Active",
                        OrgId = Convert.ToInt32(User.FindFirst("OrgID").Value)                        
                    };
                    await _context.teamMembers.AddAsync(teammem);
                    await _context.SaveChangesAsync();
                    System.Console.WriteLine($"det==>{i}");
                }
                return RedirectToPage("../Manager/AddManagers", new { projID = projeID, teamID = postteamID });
        }
    }



}