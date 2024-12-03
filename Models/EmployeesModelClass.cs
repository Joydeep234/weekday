using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace weekday.Models
{
    public class EmployeesModelClass
    {
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
        public string CPassword { get; set; }

    
        public int Designation { get; set; }
        public string Status { get; set; }

        public IFormFile  ImageURL { get; set; }
    }
}