using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using weekday.Data.Context;
using weekday.Data.Entity;
using weekday.Middleware;

namespace weekday.Pages
{
    [Authorize (Policy = "NoDesignationOrManager")]
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

        [BindProperty]
        [Required]
        public string orgName { get; set; }
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

        public async Task<IActionResult> OnPostConfirmation(int planid,int planexp){
            if(!ModelState.IsValid)throw new CustomExceptionClass("data shoul be filled");
            var userid = User.FindFirst("empID")?.Value;
            if(userid==null)return RedirectToPage("/Login");
            var org = new Organization{
                OrgName = orgName,
                PurchasedDate = DateTime.Now,
                PurchaseExpiration= DateTime.Now.AddDays(planexp),
                ClientID = Convert.ToInt32(userid),
                PlanID = planid,
            };

            await _context.organization.AddAsync(org);
            await _context.SaveChangesAsync();

            var desigid = await _context.designation.Where(d=>d.Name=="MANAGER").FirstOrDefaultAsync();
            
            if(userid == null)throw new Exception("employee id not found");
            var user = Convert.ToInt32(userid);
            var employee = await _context.employee.Where(e=>e.EmployeeId == user).FirstOrDefaultAsync();

            employee.OrgId = org.OrgId;
            employee.DesignationId = Convert.ToInt32(desigid.DesignationId);

            await _context.SaveChangesAsync();

           var claims = new List<Claim>
            {
                new Claim("empID", userid),
                new Claim("OrgID", org.OrgId.ToString()),
                new Claim("DesigName", "MANAGER"),
                new Claim("DesigID", desigid.ToString()),
                new Claim("PlanID", planid.ToString())
            };

                var claimsIdentity = new ClaimsIdentity(claims, "Cookies");
                var authProperties = new AuthenticationProperties { IsPersistent = true };

                // Sign out the current user
                await HttpContext.SignOutAsync("Cookies");

                // Sign in the user with updated claims
                await HttpContext.SignInAsync("Cookies", new ClaimsPrincipal(claimsIdentity), authProperties);

                return RedirectToPage("/PurchaseConfirmation");
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