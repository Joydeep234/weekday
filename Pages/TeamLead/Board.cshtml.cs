using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace weekday.Pages.TeamLead
{
    public class BoardModel : PageModel
    {
        public async Task<IActionResult> OnGetAsync()
        {
            return Page();
        }
    }
}



/*[HttpPost]
public IActionResult UpdateTaskOrder(string fromColumn, string toColumn, List<int> taskIds)
{
    try
    {
        // Assuming you have a Task entity and it has a Column and Order field
        var tasks = _context.Tasks.Where(t => taskIds.Contains(t.Id)).ToList();

        foreach (var task in tasks)
        {
            // Update the task's column and order in the database
            task.Column = toColumn; // Set to the new column
            task.Order = taskIds.IndexOf(task.Id); // Set the new order based on position
        }

        _context.SaveChanges();

        return Json(new { success = true });
    }
    catch (Exception ex)
    {
        return Json(new { success = false, message = ex.Message });
    }
}*/

