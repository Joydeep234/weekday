namespace weekday.Models.ProjectManagerModel
{
    using System.ComponentModel.DataAnnotations;

    public class InsertPlan
    {
        [Required(ErrorMessage = "Plan Name is required.")]
        [StringLength(100, ErrorMessage = "Plan Name must be less than 100 characters.")]
        public string PlanName { get; set; }

        [Required(ErrorMessage = "Duration is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Duration must be at least 1.")]
        public int Duration { get; set; } // Represents the number (e.g., 30 for 30 days)

         

        [Required(ErrorMessage = "Max Members is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Max Members must be at least 1.")]
        public int MaxMembers { get; set; }

        [Required(ErrorMessage = "Price is required.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be a positive value.")]
        public double Price { get; set; }
    }

    

}
