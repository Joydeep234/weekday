using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Threading.Tasks;
using weekday.Data.Context;
using weekday.Data.Entity;
using weekday.Models.Team_Member;

namespace weekday.Pages.TeamMember
{
    [Authorize(Policy = "TEAM_MEMBERS")]
    public class TaskCompletionModel : PageModel
    {
        private readonly AppDbcontext _context;

        public List<ProjectTask> TaskList {  get; set; }=new List<ProjectTask>();

        public TaskCompletionModel(AppDbcontext context)
        {
            _context = context;
            
        }
        [BindProperty]
        public TaskMdl TaskMdIP { get; set; }
        
        public string PrjName {  get; set; }// 29/11/2024

        public async Task<IActionResult> OnGetAsync(int taskId,string ProjectName)
        {
            PrjName = ProjectName;
            TaskList = _context.projecttask.ToList();
            var validTask = await _context.projecttask.FirstOrDefaultAsync(p=>p.TaskId == taskId);
            if (validTask != null) {
                TaskMdIP = new TaskMdl
                {
                    TaskIdM=validTask.TaskId,
                    TitleM=validTask.Title,
                    AssignedByIdM=validTask.AssignedById,
                    AssignedDateM = validTask.AssignedDate,
                    AssignedForIdM=validTask.AssignedForId,
                    DeadlineM = validTask.Deadline,
                    SummaryM =validTask.Summary,
                    DetailsM=validTask.Details,
                    StatusM=validTask.Status,
                    LatestUpdateTimeM=validTask.LatestUpdateTime,
                };
            }
            return Page();
        }


        public async Task<IActionResult> OnPostAsync()
        {
            
                var editWork = await _context.projecttask.FirstOrDefaultAsync(p => p.TaskId == TaskMdIP.TaskIdM);
                if (editWork != null) {
                    editWork.Status = TaskMdIP.StatusM.ToUpper();
                    editWork.LatestUpdateTime = DateTime.Now;
                    if (TaskMdIP.StatusM.ToUpper() == "DONE")
                    {
                        editWork.EndDate = DateTime.Now;
                    }
                    _context.SaveChanges();
                    TempData["Success Message"] = "Status Updated Successfully";
                    return RedirectToPage("/TeamMember/DashBoard", new {prjid=editWork.ProjectId,prjName= PrjName });  //29/11/2024 
                    //int prjid, string prjName
                    //return RedirectToPage("/TeamMember/ProjectDashBoard");
                }
            

            return Page();
        }
    }
}
