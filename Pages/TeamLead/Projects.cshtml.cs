using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using weekday.Data.Context;
using weekday.Models;
using weekday.Data.Entity;
using Microsoft.AspNetCore.Authorization;

namespace weekday.Pages.Team_Lead
{
    [Authorize(Policy = "TEAM_LEAD")]
    public class ProjectsModel : PageModel
    {
        private readonly AppDbcontext _context;

        public ProjectsModel(AppDbcontext context)
        {
            _context = context;
        }

        [BindProperty]
        public CreateTeam createTeam { get; set; }

        public List<ProjectDetails> projectDetails { get; set; } = new List<ProjectDetails>();

        public List<Employee> employees { get; set; } = new List<Employee>();

        public List<Team> teamList { get; set; } = new List<Team>();

        public void OnGet()
        {
            
            projectDetails = (from p in _context.project
                                    join t in _context.team on p.ProjectId equals t.ProjectId
                                    join tM in _context.teamMembers on t.TeamId equals tM.TeamId
                                    join e in _context.employee on t.ManagerId equals e.EmployeeId 
                                    where tM.MemberId == 3 // Assuming 3 is the current Team Member ID (e.g., Team Lead)
                                    select new ProjectDetails
                                    {
                                        ProjectId = p.ProjectId,
                                        ProjectName = p.Name,
                                        About = p.About,
                                        StartDate = p.StartDate,
                                        EndDate = p.EndDate,
                                        Deadline = p.Deadline,
                                        CreatedByID = p.CreatedByID,
                                        Details = p.Details,
                                        ProjectStatus = p.Status,
                                        ProjectOrgId = p.OrgId,
                                        ManagerId = t.ManagerId,
                                        ManagerFirstName = e.FirstName,  
                                        ManagerLastName = e.LastName,    
                                        ManagerEmail = e.Email           
                                    }).Distinct()
                                    .ToList();

            employees = (from E in _context.employee
                               join D in _context.designation
                               on E.DesignationId equals D.DesignationId
                               where D.Name == "TEAM_MEMBER"
                               select E)
                               .ToList();

            //if (TempData["designationId"] == null)
            //{
            //    TempData["designationId"] = 0;
            //}
            //TempData["designationId"] = employees[0].DesignationId;

        }

        public async Task<JsonResult> OnGetTeamList(int ProjectId)
        {
            teamList = await (from t in _context.team
                              where t.ProjectId == ProjectId
                              select t).ToListAsync();
            
            return new JsonResult(teamList);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult OnPostCreateOrUpdate([FromBody] CreateTeam team)
        {
           
            foreach(var memberId in team.MembersId)
            {
                bool alreadyExist = _context.teamMembers.Any(x => x.TeamId == team.TeamId && x.MemberId == memberId);

                if (!alreadyExist)
                {
                    TeamMembers teamMember = new TeamMembers()
                    {
                        TeamId = team.TeamId,
                        MemberId = memberId,
                        //DesignationId = Convert.ToInt32(TempData["designationId"]),
                        status = "Active",
                        OrgId = 2
                    };
                     _context.teamMembers.Add(teamMember);
                    _context.SaveChanges();
                }
                else
                {
                    Console.WriteLine("Team already Exist");
                }

            }  
            
            return new JsonResult(new { success = true });
        }

    }

    public class ProjectDetails
    {
        public int ProjectId { get; set; }
        public string ProjectName { get; set; }
        public string About { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime Deadline { get; set; }
        public int CreatedByID { get; set; }
        public string Details { get; set; }
        public string ProjectStatus { get; set; }
        public int ProjectOrgId { get; set; }
        public int ManagerId { get; set; }
        public string ManagerFirstName { get; set; }
        public string ManagerLastName { get; set; }
        public string ManagerEmail { get; set; }
    }

}