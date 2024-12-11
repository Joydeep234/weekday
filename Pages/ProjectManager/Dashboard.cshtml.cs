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
using weekday.Models.ProjectManagerModel;

namespace weekday.Pages.Project_Manager
{
    [Authorize (Policy ="PROJECT_MANAGER")]
    public class Dashboard : PageModel
    {

        public readonly AppDbcontext _context;

        public Dashboard(AppDbcontext context)
        {
            _context = context;
        }
        [BindProperty]
        public ProjectDisplay projectDisplay { get; set; }

        public List<ProjectDisplay> employeeProjects = new List<ProjectDisplay>();

        public void  OnGet()
        {
            employeeProjects = (from project in _context.project
                                join team in _context.team on project.ProjectId equals team.ProjectId
                                join teamMember in _context.teamMembers on team.TeamId equals teamMember.TeamId
                                join Employee in _context.employee on teamMember.MemberId equals Employee.EmployeeId
                                where Employee.EmployeeId == Convert.ToInt32(User.FindFirst("empID").Value)
                                select new ProjectDisplay
                                {
                                    ProjectId = project.ProjectId,
                                    ProjectName = project.Name,
                                    About = project.About,
                                    ProjectStatus = project.Status,
                                    StartDate = project.StartDate,
                                    EndDate = project.EndDate,
                                    Deadline = project.Deadline,
                                    Project_Details = project.Details,
                                   /* EmpImag = Employee.ImageURL,*/
                                    
                                }).Distinct().ToList();

            Console.WriteLine(employeeProjects);
        }
    }
}