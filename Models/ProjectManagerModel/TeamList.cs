using System.ComponentModel.DataAnnotations;

namespace weekday.Models.ProjectManagerModel
{
    public class TeamList
    {
        public int TeamId { get; set; }

        public int ProjectId { get; set; }

        public string TeamName { get; set; }

        public string TeamStatus { get; set; }

    }
}
