
using System.ComponentModel.DataAnnotations;

public class CreateTask
{
    [Required(ErrorMessage = "Please select a project.")]
    [Range(1, int.MaxValue, ErrorMessage = "Please select a valid project.")]
    public int ProjectId { get; set; }

    [Required(ErrorMessage = "Please select a team.")]
    [Range(1, int.MaxValue, ErrorMessage = "Please select a valid team.")]
    public int TeamId { get; set; }

    [Required(ErrorMessage = "Please select a task type.")]
    [RegularExpression("^(Bug|Feature|Improvement)$", ErrorMessage = "Please select a valid task type.")]
    public string TaskType { get; set; } 

    [Required(ErrorMessage = "Please select an assignee.")]
    [Range(1, int.MaxValue, ErrorMessage = "Please select an assignee.")]
    public int Assignee { get; set; } 

    [Required(ErrorMessage = "Please select a priority.")]
    [RegularExpression("^(High|Medium|Low)$", ErrorMessage = "Please select a valid priority.")]
    public string Priority { get; set; } 

    [Required(ErrorMessage = "Please provide a summary.")]
    [StringLength(200, ErrorMessage = "Summary cannot exceed 200 characters.")]
    public string Summary { get; set; }

    [Required(ErrorMessage = "Please select a status.")]
    [RegularExpression("^(To-Do|In-Progress|Done)$", ErrorMessage = "Please select a valid status.")]
    public string Status { get; set; } 

    [Required(ErrorMessage = "Please provide a description.")]
    public string Description { get; set; } // Quill or text editor input

    [Required(ErrorMessage = "Please select a start date.")]
    [DataType(DataType.Date, ErrorMessage = "Please select a valid start date.")]
    public DateTime StartDate { get; set; }

    [Required(ErrorMessage = "Please select a deadline.")]
    [DataType(DataType.Date, ErrorMessage = "Please select a valid deadline.")]
    public DateTime Deadline { get; set; }
}
