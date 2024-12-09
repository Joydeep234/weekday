using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Primitives;
using weekday.Data.Context;
using weekday.Data.Entity;

namespace weekday.Pages.TeamLead
{
    [Authorize(Policy = "TEAM_LEAD")]
    public class TaskModel : PageModel
    {
        private readonly AppDbcontext _context;

        public TaskModel(AppDbcontext context)
        {
            _context = context;
        }
        public string Reporter { get; set; }

        public string ReporterImageURL { get; set; }


        public TaskDetails? taskInfo { get; set; } = new TaskDetails();

        public void OnGet(int taskId, string imageURL)
        {
            if(taskId != 0)
            {
                taskInfo = (from T in _context.projecttask
                            join Te in _context.team
                            on T.TeamId equals Te.TeamId
                            join P in _context.project
                            on Te.ProjectId equals P.ProjectId
                            join E in _context.employee
                            on T.AssignedForId equals E.EmployeeId
                            where T.TaskId == taskId
                            && T.AssignedById ==  3 // user session Data
                            select new TaskDetails
                            {
                                taskDetails = T,
                                ProjectName = P.Name,
                                ProjectStatus = P.Status,
                                TeamName = Te.Name,
                                TeamStatus = Te.Status,
                                Assignee = E.FirstName + " " + E.LastName,
                                AssigneeImageURL = E.ImageURL,
                            }).FirstOrDefault();

                var ReporterData = (from E in _context.employee
                                    where E.EmployeeId == taskInfo.taskDetails.AssignedById
                                    select E).FirstOrDefault();
                Reporter = ReporterData?.FirstName + " " + ReporterData?.LastName;
                ReporterImageURL = ReporterData.ImageURL;
            }
            else
            {
                taskInfo = null;
            }
            
        }

        public static string GetRelativeTime(DateTime? date)
        {
            if (date == null) return "No date provided";

            var timespan = DateTime.Now - date.Value;

            if (timespan.TotalDays >= 1)
                return $"{(int)timespan.TotalDays} days ago";
            else if (timespan.TotalHours >= 1)
                return $"{(int)timespan.TotalHours} hours ago";
            else if (timespan.TotalMinutes >= 1)
                return $"{(int)timespan.TotalMinutes} minutes ago";
            else
                return "Just now";
        }

        public static string GetRelativeTimeDeadline(DateTime? deadline)
        {
            if (deadline == null) return "No deadline set";

            var now = DateTime.Now;
            var timespan = deadline.Value - now;

            if (timespan.TotalDays > 0)
                return $"{(int)timespan.TotalDays} days left";
            else if (timespan.TotalHours > 0)
                return $"{(int)timespan.TotalHours} hours left";
            else if (timespan.TotalMinutes > 0)
                return $"{(int)timespan.TotalMinutes} minutes left";
            else
                return "Deadline passed";
        }


        public class TaskDetails            
        {
            public ProjectTask taskDetails { get; set; }
            public string ProjectName { get; set; }

            public string ProjectStatus { get; set; }

            public string TeamName { get; set; }

            public string TeamStatus { get; set; }

            public string Assignee { get; set; }

            public string AssigneeImageURL { get; set; }

            public string Reporter { get; set; }

        }
    }
}
