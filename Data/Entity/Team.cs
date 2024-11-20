using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace weekday.Data.Entity
{
    public class Team
    {
        [Key]
        public int TeamId { get; set; }
        [Required]
        public int ProjectId { get; set; }
        [Required]
        public string Name { get; set; }

        public string Description { get; set; }

        public string Status { get; set; }

        public int ManagerId { get; set; }

        public int OrgId { get; set; }
    }
}