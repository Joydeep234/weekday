using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace weekday.Data.Entity
{
    public class PLans
    {
        [Key]
        public int planID { get; set; }
        public string PlanName { get; set; }
        public int Duration{ get; set; }
        public int MaxMembers { get; set; }

        public Double Price { get; set; }
    }
}