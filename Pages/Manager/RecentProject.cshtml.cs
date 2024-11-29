using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using weekday.Data.Context;
using weekday.Data.Entity;

namespace weekday.Pages.Manager
{
    public class projectandtask{
        public int projectid { get; set; }
        public string projectname { get; set; }
        public DateTime creationdate { get; set; }
        public DateTime deadline { get; set; }
        public string status { get; set; }
        public double complete { get; set; }
    }
    public class RecentProject : PageModel
    {
        private readonly AppDbcontext _context;
        private readonly ILogger<RecentProject> _logger;

        public RecentProject(ILogger<RecentProject> logger,AppDbcontext dbcontext)
        {
            _logger = logger;
            _context = dbcontext;
        }

        public List<Project> AllprojectData {get; set;} = new List<Project>();
        public List<projectandtask> projectandtask {get; set;} = new List<projectandtask>();
        public List<ProjectTask> task {get; set;} = new List<ProjectTask>();
        public async Task OnGet()
        {
            try
            {
                AllprojectData = await _context.project.Where(p=>p.OrgId==2 && p.Status!="Completed").ToListAsync();
                if(AllprojectData.Count<1)throw new Exception("Product cannot fetch from db");
                
                foreach(var team in AllprojectData){
                    var tasklist = await _context.projecttask.Where(t=>t.ProjectId == team.ProjectId).Select(t=>new { t.AssignedDate, t.EndDate }).ToListAsync();
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

                    var tandt = new projectandtask{
                        projectid = team.ProjectId,
                        projectname=team.Name,
                        creationdate=team.CreatedAt,
                        deadline = team.Deadline,
                        status = team.Status,
                        complete = percent,
                    };
                    projectandtask.Add(tandt);
                }
            }
            catch (Exception e)
            {
               Console.WriteLine($"products fetching error=>{e.Message}");
            }
        }
    }
}