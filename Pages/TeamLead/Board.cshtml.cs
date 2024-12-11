using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using weekday.Data.Context;
using weekday.Data.Entity;
using weekday.Middleware;
using weekday.Models;

namespace weekday.Pages.TeamLead
{
    [Authorize (Policy ="TEAM_LEAD")]
    public class BoardModel : PageModel
    {
        private readonly AppDbcontext _context;

        public BoardModel(AppDbcontext context)
        {
            _context = context;
        }

        [BindProperty]
        public CreateTeam createTeam { get; set; }

        [BindProperty]
        public CreateTask createTask { get; set; }

        public List<ProjectDetails> projectDetails { get; set; } = new List<ProjectDetails>();

        public List<Employee> employees { get; set; } = new List<Employee>();

        public List<ProjectTeamMembers> projectTeamMembers { get; set; } = new List<ProjectTeamMembers>(); 
        public List<Team> teamList { get; set; } = new List<Team>();

        public List<ProjectTasks> projectTasks { get; set; } = new List<ProjectTasks>();

        public int randomDigitNonZero;

        public int TL_id { get; set; }
        public int Org_id { get; set; }
        public string? DesignationName { get; set; }
        public int Designation_id { get; set; }


        public IActionResult OnGet()
        {
            Random random = new Random();
            randomDigitNonZero = random.Next(1, 10);

            TL_id = Convert.ToInt32(User.FindFirst("empID")?.Value ?? throw new CustomExceptionClass("Employee ID claim not found"));
            Org_id = Convert.ToInt32(User.FindFirst("OrgID")?.Value ?? throw new CustomExceptionClass("Organization ID claim not found"));
            DesignationName = User.FindFirst("DesigName")?.Value ?? throw new CustomExceptionClass("Designation Name claim not found");
            Designation_id = Convert.ToInt32(User.FindFirst("DesigID")?.Value ?? throw new CustomExceptionClass("Designation ID claim not found"));


            projectDetails = (from p in _context.project
                              join t in _context.team on p.ProjectId equals t.ProjectId
                              join tM in _context.teamMembers on t.TeamId equals tM.TeamId
                              join e in _context.employee on t.ManagerId equals e.EmployeeId
                              where tM.MemberId == TL_id && tM.OrgId ==  Org_id // Assuming 3 is the current Team Member ID (e.g., Team Lead)
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
                              })
                              .Distinct()
                              .ToList() ?? throw new CustomExceptionClass("No project details found");


            employees = (from E in _context.employee
                             join D in _context.designation
                             on E.DesignationId equals D.DesignationId
                             where D.Name == "TEAM_MEMBERS" && E.OrgId == Org_id
                             select E)
                             .ToList() ?? throw new CustomExceptionClass("No team members found");


            projectTasks = (from Ta in _context.projecttask
                            join T in _context.team
                            on Ta.TeamId equals T.TeamId
                            join P in _context.project
                            on Ta.ProjectId equals P.ProjectId
                            join E in _context.employee
                            on Ta.AssignedForId equals E.EmployeeId
                            where (Ta.AssignedById == TL_id)
                            && Ta.OrgId == Org_id
                            orderby Ta.AssignedDate descending
                            select new ProjectTasks
                            {
                                taskDetails = Ta,
                                ProjectName = P.Name,
                                ProjectStatus = P.Status,
                                TeamName = T.Name,
                                TeamStatus = T.Status,
                                AssigneeImageURL = E.ImageURL
                            }).ToList() ?? throw new CustomExceptionClass("No project tasks found");

            //TempData["designationId"] = employees[0].DesignationId;
            TempData["designationId"] = employees.FirstOrDefault()?.DesignationId ?? 0;

            return Page();
        }

        public async Task<IActionResult> OnGetProjectTeamMembers(int ProjectId, int TeamId)
        {
            TL_id = Convert.ToInt32(User.FindFirst("empID")?.Value ?? throw new CustomExceptionClass("Employee ID claim not found"));
            Org_id = Convert.ToInt32(User.FindFirst("OrgID")?.Value ?? throw new CustomExceptionClass("Organization ID claim not found"));
            DesignationName = User.FindFirst("DesigName")?.Value ?? throw new CustomExceptionClass("Designation Name claim not found");
            Designation_id = Convert.ToInt32(User.FindFirst("DesigID")?.Value ?? throw new CustomExceptionClass("Designation ID claim not found"));


            projectTeamMembers = await (from T in _context.team
                                        join Tm in _context.teamMembers on T.TeamId equals Tm.TeamId
                                        join E in _context.employee on Tm.MemberId equals E.EmployeeId
                                        join D in _context.designation on E.DesignationId equals D.DesignationId
                                        where T.ProjectId == ProjectId && T.TeamId == TeamId && D.Name == "TEAM_MEMBERS" && E.OrgId == Org_id
                                        select new ProjectTeamMembers
                                        {
                                            EmployeeId = E.EmployeeId,
                                            FirstName = E.FirstName,
                                            LastName = E.LastName,
                                            ImageURL = E.ImageURL,
                                        }).ToListAsync() ?? throw new CustomExceptionClass("No Projects Found");

            
            if(projectTeamMembers.Count != 0)
            {
                return new JsonResult(new { success = true, teamMembers = projectTeamMembers });
            }
            else
            {
                return new JsonResult(new { success = false });
            }
        }

        public async Task<JsonResult> OnGetTeamList(int ProjectId)
        {
            teamList = await (from t in _context.team
                              where t.ProjectId == ProjectId
                              select t).ToListAsync() ?? throw new CustomExceptionClass("No teams found");

            return new JsonResult(teamList);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult OnPostCreateOrUpdate([FromBody] CreateTeam team)
        {

            TL_id = Convert.ToInt32(User.FindFirst("empID")?.Value ?? throw new CustomExceptionClass("Employee ID claim not found"));
            Org_id = Convert.ToInt32(User.FindFirst("OrgID")?.Value ?? throw new CustomExceptionClass("Organization ID claim not found"));
            DesignationName = User.FindFirst("DesigName")?.Value ?? throw new CustomExceptionClass("Designation Name claim not found");
            Designation_id = Convert.ToInt32(User.FindFirst("DesigID")?.Value ?? throw new CustomExceptionClass("Designation ID claim not found"));


            List<TeamMembers> newTeamMembers = new List<TeamMembers>();
                foreach (var memberId in team.MembersId)
                {
                    bool alreadyExist = _context.teamMembers.Any(x => x.TeamId == team.TeamId && x.MemberId == memberId);


                    if (!alreadyExist && memberId != 0)
                    {
                        newTeamMembers.Add(new TeamMembers
                        {
                            TeamId = team.TeamId,
                            MemberId = memberId,
                            DesignationId = Convert.ToInt32(TempData["designationId"]),
                            status = "Active",
                            OrgId = Org_id
                        });

                        Console.WriteLine("Team Added successfully");
                    }

                    else
                    {
                        return new JsonResult(new { success = false });
                    }

                }
                if (newTeamMembers.Any())
                {
                    TempData["ToastMessage"] = "Success!, Team Created..";
                    TempData["ToastType"] = "success";
                    _context.teamMembers.AddRange(newTeamMembers);
                    _context.SaveChanges();
                    
                return new JsonResult(new { success = true });
                }
            
            return new JsonResult(new { success = false });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> OnPostCreateOrUpdateTask([FromBody] CreateTask taskData)
        {
            TL_id = Convert.ToInt32(User.FindFirst("empID")?.Value ?? throw new CustomExceptionClass("Employee ID claim not found"));
            Org_id = Convert.ToInt32(User.FindFirst("OrgID")?.Value ?? throw new CustomExceptionClass("Organization ID claim not found"));
            DesignationName = User.FindFirst("DesigName")?.Value ?? throw new CustomExceptionClass("Designation Name claim not found");
            Designation_id = Convert.ToInt32(User.FindFirst("DesigID")?.Value ?? throw new CustomExceptionClass("Designation ID claim not found"));

            if (taskData != null)
            {
                ProjectTask task = new ProjectTask
                {
                    ProjectId = taskData.ProjectId,
                    TeamId = taskData.TeamId,
                    Title = taskData.TaskType,
                    Summary = taskData.Summary,
                    Details = taskData.Description,
                    Status = taskData.Status,
                    Priority = taskData.Priority,
                    AssignedById = TL_id, // TL id from session
                    AssignedForId = taskData.Assignee,
                    AssignedDate = DateTime.Now,
                    StartDate = taskData.StartDate,
                    Deadline = taskData.Deadline,
                    OrgId = Org_id
                };

                await _context.projecttask.AddAsync(task);
                await _context.SaveChangesAsync();

                return new JsonResult(new { success = true, message = "Success! Team Created.", type = "success" });
            }
            else
            {    
                return new JsonResult(new { success = false });
            }   
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult OnPostUpdateTaskOrder(string fromColumn, string toColumn, List<int> taskIds)
        {

            TL_id = Convert.ToInt32(User.FindFirst("empID")?.Value ?? throw new CustomExceptionClass("Employee ID claim not found"));
            Org_id = Convert.ToInt32(User.FindFirst("OrgID")?.Value ?? throw new CustomExceptionClass("Organization ID claim not found"));
            DesignationName = User.FindFirst("DesigName")?.Value ?? throw new CustomExceptionClass("Designation Name claim not found");
            Designation_id = Convert.ToInt32(User.FindFirst("DesigID")?.Value ?? throw new CustomExceptionClass("Designation ID claim not found"));



            var tasks = _context.projecttask.Where(t => taskIds.Contains(t.TaskId)).ToList() ?? throw new CustomExceptionClass("No tasks found");
            if(tasks.Count != 0)
            {
                foreach (var task in tasks)
                {
                    
                    task.Status = toColumn; // Set to the new column
                    //task.Order = taskIds.IndexOf(task.Id); // Set the new order based on position
                }

                _context.SaveChanges();
                return new JsonResult(new { success = true });
            }
            else
            {
                return new JsonResult(new { success = false});
            }     
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

    public class ProjectTeamMembers
    {
        public int EmployeeId { get; set; }
        public string FirstName { get; set; }

        public string LastName { get; set; }   

        public string ImageURL { get; set; }

    }

    public class ProjectTasks 
    {
        public ProjectTask taskDetails { get; set; }
        public string ProjectName { get; set; }

        public string ProjectStatus { get; set; }

        public string TeamName { get; set;}

        public string TeamStatus { get; set; }

        public string AssigneeImageURL { get; set; }

    }
}





