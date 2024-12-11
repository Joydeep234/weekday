using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using System.Diagnostics;
using System.Reflection.Metadata.Ecma335;
using weekday.Data.Context;
using weekday.Data.Entity;
using weekday.Models.ProjectManagerModel;

namespace weekday.Pages.ProjectManager
{
    [Authorize (Policy ="PROJECT_MANAGER")]
    public class TeamsModel : PageModel
    {

        public readonly AppDbcontext _context;

        public TeamsModel(AppDbcontext context)
        {
            _context = context;
        }

        public List<teams_project_display> teams_Project_Display { get; set; }
        public async Task<IActionResult> OnGetAsync()
        {
            teams_Project_Display = await (from project in _context.project
                                           join team in _context.team
                                           on project.ProjectId equals team.ProjectId
                                           join teamMember in _context.teamMembers on team.TeamId equals teamMember.TeamId
                                           join employee in _context.employee on teamMember.MemberId equals employee.EmployeeId
                                           where employee.EmployeeId == Convert.ToInt32(User.FindFirst("empID").Value)
                                           select new teams_project_display { 
                                               TeamId = team.TeamId,
                                               TeamName = team.Name,
                                               TeamStatus = team.Status,
                                               ProjectName = project.Name,
                                           }).ToListAsync();

            



            return Page();
        }
        public async Task<IActionResult> OnGetTeamDispalyAsync(int teamId)
        {
            if (teamId != 0)
            {
                var team_details = await _context.team.FindAsync(teamId);
                int projectId = team_details.ProjectId;
                var project_details = await _context.project.FindAsync(projectId);


                return new JsonResult(new { success = true, team = team_details, proj = project_details });
            }
            return new JsonResult(new { success = false });
        }
        public async Task<IActionResult> OnGetTeamMemberDispalayAsync(int teamsId)
        {
            if (teamsId != 0)
            {
                var memberIds = _context.teamMembers
                        .Where(tm => tm.TeamId == teamsId)
                        .Select(tm => tm.MemberId)
                        .ToList();

                var memberDetails = _context.employee
                                .Where(e => memberIds.Contains(e.EmployeeId)) // Fetch only matching IDs
                                .Select(e => new
                                {
                                    EmployeeId = e.EmployeeId,
                                    FirstName = e.FirstName,
                                    LastName = e.LastName,
                                    Email = e.Email,
                                    Status = e.Status,
                                    ImageURL = e.ImageURL
                                })
                                .ToList();

                return new JsonResult(new { success = true , emp_det = memberDetails });

            }
            return new JsonResult(new { success = false });
        }
    }
}
