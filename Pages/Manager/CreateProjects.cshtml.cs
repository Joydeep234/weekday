using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using weekday.Data.Context;
using weekday.Data.Entity;
using weekday.Middleware;
using weekday.Models;

namespace weekday.Pages.Manager
{
    [Authorize (Policy ="MANAGER")]
    public class CreateProjects : PageModel
    {
        private readonly AppDbcontext _context;
        private readonly ILogger<CreateProjects> _logger;

        public CreateProjects(ILogger<CreateProjects> logger,AppDbcontext dbcontext)
        {
            _logger = logger;
            _context = dbcontext;
        }
        [BindProperty]
        public ProjectModelClass projectmodel { get; set; }
        public Project projectData { get; set; } = new Project();
        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAddProject(){
            var empidClaims = User.FindFirst("empID");
            var orgidclaims = User.FindFirst("OrgId");
            Int32 empid = new Int32();
            Int32 orgid=new Int32();
            if(empidClaims!=null){
                empid = Convert.ToInt32(empidClaims.Value);
            }
            if(orgidclaims!=null){
                orgid = Convert.ToInt32(orgidclaims.Value);
            }
             if(!ModelState.IsValid)throw new CustomExceptionClass("Some feild is empty");
                var project = new Project{
                    Name = projectmodel.Name,
                    About = projectmodel.About,
                    CreatedAt = projectmodel.CreatedAt,
                    Deadline = projectmodel.Deadline,
                    CreatedByID = empid,
                    Details = projectmodel.Details,
                    Status = projectmodel.Status.ToString(),
                    OrgId = orgid
                };
                await _context.project.AddAsync(project);
                await _context.SaveChangesAsync();
                System.Console.WriteLine($"project id ==>{project.ProjectId}");
                return RedirectToPage("../Manager/RecentProject");
        }
    }
}