using System.ComponentModel.DataAnnotations;

namespace weekday.Models.Team_Member
{
    public class TaskMdl
    {
        
        public int TaskIdM { get; set; }

        
        public int ProjectIdM { get; set; }

        
        public string TitleM { get; set; }

        
        public string SummaryM { get; set; }

        public string DetailsM { get; set; }

        public string? StatusM { get; set; }

        
        public int AssignedForIdM { get; set; }

        
        public int AssignedByIdM { get; set; }

        
        public DateTime AssignedDateM { get; set; }

        public DateTime? LatestUpdateTimeM { get; set; }
        public DateTime? StartDateM { get; set; }

        public DateTime? EndDateM { get; set; }

        
        public DateTime DeadlineM { get; set; }

        public int OrgIdM { get; set; }
    }
}
