using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace weekday.Models
{
    public class ProjectModelClass
    {
        
        [Required, StringLength(100)]
        public string? Name { get; set; }

        [Required(ErrorMessage = "Field is Needed")]
        [StringLength(maximumLength:100,ErrorMessage ="Maximum Length should be 100")]
        public string? About { get; set; }
        [Required(ErrorMessage = "Field is Needed")]
        public DateTime CreatedAt { get; set; }

        [Required(ErrorMessage = "Field is Needed")]
        public DateTime Deadline { get; set; }

        [Required(ErrorMessage = "Field is Needed")]
        public string? Details { get; set; }
        [Required(ErrorMessage = "Select Proper Status Type")]
        public ProjectStatus Status { get; set; }
    }

    public enum ProjectStatus{
        ToDo,
        InProgress,
        Completed
    } 
}