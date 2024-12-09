using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using weekday.Data.Context;
using weekday.Data.Entity;
using weekday.Models.Team_Member;

namespace weekday.Pages.ProjectManager
{
    [Authorize (Policy ="PROJECT_MANAGER")]
    public class TaskDashboardModel : PageModel
    {
        public readonly AppDbcontext _context;

        public TaskDashboardModel(AppDbcontext context)
        {
            _context =  context;
        }

        public List<TaskList> taskLists {  get; set; }
        public async Task<IActionResult> OnGetAsync()
        {
            taskLists = await (from task in _context.projecttask
                               join Employee in _context.employee
                               on task.AssignedById equals Employee.EmployeeId
                               select new TaskList
                               {
                                   TaskId = task.TaskId,
                                   summary = task.Summary,
                                   taskStatus = task.Status,
                                   AssignedByFristName = Employee.FirstName,
                                   AssignedByLastName = Employee.LastName,
                                   image = Employee.ImageURL,
                                   DueDate = task.EndDate ?? DateTime.MinValue,
                                   Priority = task.Priority,
                               }).ToListAsync();
            return Page();
        }



    }
    public class TaskList()
    {
        public int TaskId { get; set; }

        public string summary { get; set; }
        public string taskStatus { get; set; }
        public DateTime DueDate { get; set; }
        public string Priority { get; set; }

        public string AssignedByFristName { get; set; }
        public string AssignedByLastName { get; set; }
        public string image {  get; set; }
    }
}
