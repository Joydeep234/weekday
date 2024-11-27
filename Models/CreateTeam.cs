using System.ComponentModel.DataAnnotations;


namespace weekday.Models
{
    public class CreateTeam
    {
        [Required(ErrorMessage = "Please select a team")]
        [Range(1, int.MaxValue, ErrorMessage = "Please select a valid team." )]
        public int TeamId { get; set; }

        //public string TeamName { get; set; }

        [Required(ErrorMessage = "Please select a project.")]
        [Range(1, int.MaxValue, ErrorMessage = "Please select a valid project.")]
        public int ProjectId { get; set; }

        //public string ProjectName { get; set; }

        [Required(ErrorMessage = "Please select at least one team member.")]
        [MinLength(1, ErrorMessage = "Please select at least one team member.")]
        public List<int> MembersId { get; set; }

    }
}
