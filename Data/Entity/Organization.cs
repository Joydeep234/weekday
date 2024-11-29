using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace weekday.Data.Entity
{
    public class Organization
    {
        [Key]
        public int OrgId { get; set; }

        [Required]
        public int ClientID { get; set; }
        [Required]
        public string OrgName { get; set; }

        public DateTime PurchasedDate { get; set; }

        public int PlanID { get; set; }

        public DateTime PurchaseExpiration { get; set; }
    }
}