using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace weekday.Data.Entity
{
    public class Designation
    {
        [Key]
        public int DesignationId { get; set; }

        [Required]
        public string Name { get; set; }

        public int OrgId { get; set; }
    }
}