using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;

namespace weekday.Data.Entity
{
    public class Project
    {
        [Key]
        public int ProjectId { get; set; }

        [Required, StringLength(100)]
        public string Name { get; set; }

        [StringLength(100)]
        public string About { get; set; }

        public DateTime CreatedAt { get; set; }
        
        public DateTime StartDate { get; set; }

        public DateTime UpdatedAt { get; set; }
        public DateTime EndDate { get; set; }

        [Required]
        public DateTime Deadline { get; set; }

        [Required]
        public int CreatedByID { get; set; }

        [Required]
        public string Details { get; set; }

        public string Status { get; set; }

        public int OrgId { get; set; }



    }
}