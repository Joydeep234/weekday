using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using weekday.Data.Context;
using weekday.Data.Entity;
using weekday.Models.Team_Member;

namespace weekday.Pages.TeamMembers
{
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

        public async Task<IActionResult> OnGetAsync(int taskId)
        {
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
            if (ModelState.IsValid) {
                var editWork = await _context.projecttask.FirstOrDefaultAsync(p => p.TaskId == TaskMdIP.TaskIdM);
                if (editWork != null) {
                    editWork.TaskId = TaskMdIP.TaskIdM;
                    editWork.Status = TaskMdIP.StatusM.ToUpper();
                    editWork.LatestUpdateTime = DateTime.Now;
                    editWork.Details=TaskMdIP.DetailsM;
                    if (TaskMdIP.StatusM.ToUpper() == "DONE")
                    {
                        editWork.EndDate = DateTime.Now;
                    }
                    _context.SaveChanges();
                    return RedirectToPage("/TeamMembers/DashBoard");
                }
            
            
            }

            return Page();
        }
        
    }
}
