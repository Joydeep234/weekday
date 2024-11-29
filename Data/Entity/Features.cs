using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace weekday.Data.Entity
{
    public class Features
    {
        [Key]
        public int FeatureID {get;set;}
        public string FeatureName {get;set;}
        public string FeatureURL {get;set;}
        public string FeatureLogo {get;set;}
        [Required]
        public int designationId {get;set;}

    }
}