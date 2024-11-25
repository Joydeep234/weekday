namespace weekday.Models
{
    public class CreateTeam
    {
        public int TeamId { get; set; }

        public string TeamName { get; set; }
        public int ProjectId { get; set; }

        public string ProjectName { get; set; }

        public List<int> MembersId { get; set; }

    }
}
