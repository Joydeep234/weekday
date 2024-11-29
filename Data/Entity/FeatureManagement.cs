using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace weekday.Data.Entity
{
    public class FeatureManagement
    {
        [Key]
        public int FmID { get; set; }
        [Required]
        public int PlanID { get; set; }
        [Required]
        public int FeatureID { get; set; }
    }
}