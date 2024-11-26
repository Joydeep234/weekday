using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using weekday.Data.Context;
using weekday.Data.Entity;
using weekday.Models;

namespace weekday.Pages.Manager
{
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
            try
            {
                if(!ModelState.IsValid)throw new Exception("Some feild is empty");
                var project = new Project{
                    Name = projectmodel.Name,
                    About = projectmodel.About,
                    CreatedAt = projectmodel.CreatedAt,
                    Deadline = projectmodel.Deadline,
                    CreatedByID = 1,
                    Details = projectmodel.Details,
                    Status = projectmodel.Status.ToString(),
                    OrgId = 2
                };
                await _context.project.AddAsync(project);
                await _context.SaveChangesAsync();
                System.Console.WriteLine($"project id ==>{project.ProjectId}");
                return RedirectToPage("../Manager/RecentProject");
            }
            catch (Exception e)
            {
                Console.WriteLine($"Create Project Error => {e.Message}");
                return Page();
            }
        }
    }
}