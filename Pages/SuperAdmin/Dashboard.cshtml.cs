using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Identity.Client;
using weekday.Data.Context;
using weekday.Data.Entity;
using weekday.Models.ProjectManagerModel;
using weekday.Pages.ProjectManager;

namespace weekday.Pages.SuperAdmin
{
    public class Dashboard : PageModel
    {
        public readonly AppDbcontext _context;

        public Dashboard(AppDbcontext context)
        {
            _context = context;
        }

        [BindProperty]
        public FeatureInsert featureInsert { get; set; }

        [BindProperty]
        public InsertPlan insertPlan { get; set; }

        public FeatureData FeatureData { get; set; }

        public List<Features> features { get; set; }

        public PlanData PlanData { get; set; }
        public void OnGet()
        {

            features = _context.feature.ToList();

        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> OnPostInsertFeature([FromBody] FeatureData formData) {


            Console.WriteLine($"FeatureName: {formData.FeatureName}, FeatureURL: {formData.FeatureURL}, FeatureLogo: {formData.FeatureLogo}");

            if (formData !=  null)
            {
                var newFeature = new Features
                {
                    FeatureName = formData.FeatureName,
                    FeatureURL = formData.FeatureURL,
                    FeatureLogo = formData.FeatureLogo,
                    designationId = 0
                };

                _context.feature.Add(newFeature);
                await _context.SaveChangesAsync();
                return new JsonResult(new { success = true });
            }



            return new JsonResult(new { success = false });
        }


        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> OnPostAddPlan([FromBody] PlanData formData)
        {

            Console.WriteLine($"PlanName: {formData.PlanName}, Duration: {formData.Duration}, Maxmembers: {formData.MaxMembers}, Price : {formData.Price} ");

           
            if (formData != null)
            {
                var newPlan = new PLans
                {
                    PlanName = formData.PlanName,
                    Duration = formData.Duration,
                    MaxMembers = formData.MaxMembers,   
                    Price = formData.Price,
                };

                _context.plans.Add(newPlan);
                await _context.SaveChangesAsync();

                foreach (var featureId in formData.SelectedFeatureIds)
                {
                    var planFeature = new FeatureManagement
                    {
                        PlanID = newPlan.planID,
                        FeatureID = featureId
                    };
                    _context.featuremanagement.Add(planFeature);
                }

                await _context.SaveChangesAsync();



                return new JsonResult(new { success = true });
            }

            return new JsonResult(new { success = false });
        }
    }
    public class FeatureData
    {
        public string FeatureName { get; set; }

        public string FeatureURL { get; set; }

        public string FeatureLogo { get; set; }
    }

    public class PlanData
    {
        public string PlanName { get; set; }

        public int Duration { get; set; }

        public int MaxMembers{ get; set; }

        public double Price{ get; set; }

        public List<int> SelectedFeatureIds { get; set; }
    }

}