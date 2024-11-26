using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace weekday.Models
{
    public class TeamModelClass
    {
        [Required(ErrorMessage = "Filed Required")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Filed Required")]
        public string Description { get; set; }
        [Required(ErrorMessage = "Filed Required")]
        public TeamStatus Status { get; set; }
    }
    public enum TeamStatus{
        Active,
        InActive
    } 
}