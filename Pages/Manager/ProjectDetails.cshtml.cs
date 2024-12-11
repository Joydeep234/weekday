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
using weekday.Models;

namespace weekday.Pages.Manager
{
    public class teamandtaskdetails{
        public int teamid { get; set; }
        public string teamname { get; set; }
        public string teamstatus { get; set; }
        public double taskcomplete { get; set; }
    }
    [Authorize (Policy ="MANAGER")]
    public class ProjectDetails : PageModel
    {
        private readonly AppDbcontext _context;
        private readonly ILogger<ProjectDetails> _logger;

        public ProjectDetails(ILogger<ProjectDetails> logger, AppDbcontext dbcontext)
        {
            _logger = logger;
            _context = dbcontext;
        }
        [BindProperty]
        public int projectID { get; set; }
        [BindProperty]
        public string projectNAME { get; set; }
        [BindProperty]
        public TeamModelClass team { get; set; }
        public bool toggleTeam { get; set; } = true;
        public Team teamDetails { get; set; }
        public ProjectAndTeam projectDetails { get; set; } = new ProjectAndTeam();
        public List<Team> teamList { get; set; } = new List<Team>();

        

        public List<teamandtaskdetails> teamandtask { get; set; } = new List<teamandtaskdetails>();
        public async Task OnGet(int projectId, string projectName)
        {
            projectID = projectId;
            projectNAME = projectName;
            try
            {
                projectDetails = await (from p in _context.project
                                        join e in _context.employee
                                        on p.CreatedByID equals e.EmployeeId
                                        join d in _context.designation
                                        on e.DesignationId equals d.DesignationId
                                        where p.ProjectId == projectId
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

                if(projectDetails == null)throw new Exception("ProjectDetails is not there");
                teamList = await _context.team.Where(p => p.ProjectId == projectId).ToListAsync();
                if (teamList.Count < 1) throw new Exception("Team is not there");
                
                foreach(var team in teamList){
                    var tasklist = await _context.projecttask.Where(t=>t.ProjectId == projectId && t.TeamId==team.TeamId).Select(t=>new { t.AssignedDate, t.EndDate }).ToListAsync();
                    var totaltaskcount = tasklist.Count;
                    
                    double percent=0;
                    var completeCount = 0;
                    foreach(var task in tasklist){
                        if(task.EndDate>task.AssignedDate){
                            completeCount++;
                        }
                    }
                    if (totaltaskcount > 0)
                    {
                        percent = (double)completeCount / totaltaskcount * 100;
                    }
                    else
                    {
                        percent = 0;
                    }

                    var tandt = new teamandtaskdetails{
                        teamid = team.TeamId,
                        teamname = team.Name,
                        teamstatus = team.Status,
                        taskcomplete = percent
                    };
                    teamandtask.Add(tandt);
                }
                
            }
            catch (Exception e)
            {
                Console.WriteLine($"team creation error==>{e.Message}");
            }
        }

        public async Task<IActionResult> OnPostToggle_Team(int projectID, string projectName)
        {
            if (toggleTeam == true) toggleTeam = false;
            return Page();
        }

        public async Task<IActionResult> OnPostCreateTeam(int projectID, string projectName)
        {
            try
            {
                Console.WriteLine($"working");
                if (!ModelState.IsValid) throw new Exception("Field cannot be empty");

                teamDetails = await _context.team.Where(t => t.ProjectId == projectID && t.Name == team.Name && t.OrgId == 2).FirstOrDefaultAsync();
                if (teamDetails != null)
                {
                    TempData["msg"] = "Team already Exists For This Project! Create Another Team";
                    return RedirectToPage("../Manager/ProjectDetails", new { projectID, projectName });
                }
                var newTeam = new Team
                {
                    ProjectId = projectID,
                    Name = team.Name,
                    Status = team.Status.ToString(),
                    Description = team.Description,
                    ManagerId = Convert.ToInt32(User.FindFirst("empID").Value),
                    OrgId = Convert.ToInt32(User.FindFirst("OrgID").Value)
                };
                await _context.team.AddAsync(newTeam);
                await _context.SaveChangesAsync();
                return RedirectToPage("../Manager/ProjectDetails", new { projectID, projectName });
            }
            catch (Exception e)
            {
                System.Console.WriteLine($"team creation error==>{e.Message}");
                return RedirectToPage("../Manager/ProjectDetails", new { projectID, projectName });
            }
        }

    }

    public class ProjectAndTeam
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string DesignationName { get; set; }
        public int ProjectId { get; set; }
        public string Name { get; set; }
        public string About { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime Deadline { get; set; }
        public string Details { get; set; }
        public string Status { get; set; }
    }
}