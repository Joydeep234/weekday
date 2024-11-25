using System.ComponentModel.DataAnnotations;

namespace weekday.Models
{
    public class ProjectDisplay
    {

        public int EmployeeId { get; set; }

        public string FirstName { get; set; }

        
        public string LastName { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }


        public int DesignationId { get; set; }

        public string EmployeeStatus { get; set; }

        public string ImageURL { get; set; }

        public int OrgId { get; set; }


        
        public int ProjectId { get; set; }

        public string ProjectName { get; set; }

        public string About { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public DateTime Deadline { get; set; }

        public int CreatedByID { get; set; }

        public string Details { get; set; }

        public string ProjectStatus { get; set; }

        public int TeamId { get; set; }
    
        public string TeamName { get; set; }

        public string Description { get; set; }

        public string TeamStatus { get; set; }

        public int ManagerId { get; set; }

        public int TeamMemberId { get; set; }

        

        public int MemberId { get; set; }

       

        public string TeamMemberstatus { get; set; }
    }
}
