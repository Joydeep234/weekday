using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using weekday.Data.Context;
using weekday.Data.Entity;

namespace weekday.Pages
{
    public class Pricing : PageModel
    {
        private readonly AppDbcontext _context;
        private readonly ILogger<Pricing> _logger;

        public Pricing(ILogger<Pricing> logger,AppDbcontext dbcontext)
        {
            _logger = logger;
            _context = dbcontext;
        }
        public List<planswithfeatures> plansfeatures { get; set; } = new List<planswithfeatures>();
        public List<PLans> plans { get; set; } = new List<PLans>();
        public List<Designation> designation { get; set; } = new List<Designation>();
        public async Task OnGet()
        {
            try
            {
                plans = await _context.plans.ToListAsync();
                if(plans.Count<1)throw new Exception("Plans Not Available");

                designation = await _context.designation.ToListAsync();

                plansfeatures = await (from p in _context.plans
                                        join fm in _context.featuremanagement on p.planID equals fm.PlanID
                                        join f in _context.feature on fm.FeatureID equals f.FeatureID
                                        join d in _context.designation on f.designationId equals d.DesignationId
                                        select new planswithfeatures{
                                            planID = p.planID,
                                            desigid = d.DesignationId,
                                            designame = d.Name,
                                            planname = p.PlanName,
                                            featureName = f.FeatureName
                                        }).ToListAsync();
                        if(plansfeatures.Count<1)throw new Exception("plansfeature is Empty");
            }
            catch (System.Exception e)
            {
                _logger.LogError(e.Message);
            }
        }
    }
    public class planswithfeatures{
        public int planID { get; set; }
        public int desigid { get; set; }
        public string designame { get; set; }
        public string planname { get; set; }
        public string featureName { get; set; }
    }
}