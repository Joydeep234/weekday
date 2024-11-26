using System.ComponentModel.DataAnnotations;

namespace weekday.Models
{
    public class ProjectDisplay
    {
        public int ProjectId { get; set; }

        public string ProjectName { get; set; }

        public string About { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public DateTime Deadline { get; set; }

        public int CreatedByID { get; set; }

        public string Project_Details { get; set; }

        public string ProjectStatus { get; set; }

        
    }
}
