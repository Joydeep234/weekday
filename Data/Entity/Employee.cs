using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace weekday.Data.Entity
{
    public class Employee
    {
        [Key]
        public int EmployeeId { get; set; }

        [Required, StringLength(100)]
        public string FirstName { get; set; }

        [Required, StringLength(100)]
        public string LastName { get; set; }

        [Required, StringLength(100)]
        public string Email { get; set; }

        [Required, StringLength(100)]
        public string Password { get; set; }

        [StringLength(50)]
        public string UserRole { get; set; }


        [StringLength(100)]
        public string Status { get; set; }

        [StringLength(200)]
        public string ImageURL { get; set; }

        public int OrgId { get; set; }
    }
}