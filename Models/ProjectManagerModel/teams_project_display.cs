namespace weekday.Models.ProjectManagerModel
{
    public class teams_project_display
    {
        public int TeamId {  get; set; }

        public string TeamName { get; set; }

        public string TeamDescription { get; set; }

        public string TeamStatus { get; set; }

        public int ProjectId { get; set; }
        public string ProjectName { get; set; }

        public string ProjectAbout { get; set; }
        public string ProjectDetails { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime StartedAt { get; set; }
        public DateTime LastUpdatedAt { get; set; }

        public DateTime EndDate { get; set; }
        public DateTime Deadline { get; set; }

        public string projectStatus { get; set; }
    }
}
