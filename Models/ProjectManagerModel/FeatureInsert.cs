using System.ComponentModel.DataAnnotations;

namespace weekday.Models.ProjectManagerModel
{
    public class FeatureInsert
    {
        [Required(ErrorMessage = "Feature Name is required.")]
        [StringLength(100, ErrorMessage = "Feature Name cannot exceed 100 characters.")]
        public string FeatureName { get; set; }

        [Required(ErrorMessage = "Feature URL is required.")]
        [Url(ErrorMessage = "Please enter a valid URL.")]
        public string FeatureURL { get; set; }

       
        public string FeatureLogo { get; set; }
    }
}
