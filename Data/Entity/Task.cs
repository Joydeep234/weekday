using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace weekday.Data.Entity
{
    public class ProjectTask
    {
        [Key]
        public int TaskId { get; set; }

        [Required]
        public int ProjectId { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Summary { get; set; }

        public string Details { get; set; }

        public string Status { get; set; }

        [Required]
        public int AssignedForId { get; set; }

        [Required]
        public int AssignedById { get; set; }

        [Required]
        public DateTime AssignedDate { get; set; }

        public DateTime LatestUpdateTime { get; set; }
        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        [Required]
        public DateTime Deadline { get; set; }

        public int OrgId { get; set; }
    }
}