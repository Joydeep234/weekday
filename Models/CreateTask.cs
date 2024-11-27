namespace weekday.Models
{
    public class CreateTask
    {
        public int ProjectId { get; set; }

        public int TeamId { get; set; }

        public int TaskType { get; set; }

        public string Priority { get; set; }

        public string Summary { get; set; }

        public string Status { get; set; }

        public string Description { get; set; } 

        public DateTime StartDate { get; set; }

        public DateTime Deadline { get; set; }
       
    }
}
