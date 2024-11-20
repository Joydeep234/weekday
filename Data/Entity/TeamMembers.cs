using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace weekday.Data.Entity
{
    public class TeamMembers
    {
        [Key]
        public int TeamMemberId { get; set; }

        public int TeamId { get; set; }

        public int MemberId { get; set; }

        public int DesignationId { get; set; }

        public string status {get; set;}

        public int OrgId { get; set; }  
    }
}