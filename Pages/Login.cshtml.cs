using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Logging;
using weekday.Data.Context;
using weekday.Data.Entity;
using weekday.Models;

namespace weekday.Pages
{
    public class Login : PageModel
    {
        private readonly AppDbcontext _context;
        private readonly ILogger<Login> _logger;

        public Login(ILogger<Login> logger,AppDbcontext dbcontext)
        {
            _logger = logger;
            _context = dbcontext;
        }

        [BindProperty]
        public LoginModelClass userlogin { get; set; } = new LoginModelClass();
        public Employee userData { get; set; }
        public Designation userDesignation { get; set; }
        public IActionResult OnGet()
        {
            if (User.Identity != null && User.Identity.IsAuthenticated)
            {
                
                var desigName = User.FindFirst("DesigName")?.Value;

                
                return desigName switch
                {
                    "HR" => RedirectToPage("HR/Dashboard"),
                    "MANAGER" => RedirectToPage("Manager/Dashboard"),
                    "PROJECT_MANAGER" => RedirectToPage("ProjectManager/Dashboard"),
                    "TEAM_LEAD" => RedirectToPage("TeamLead/Board"),
                    "TEAM_MEMBERS" => RedirectToPage("TeamMember/Dashboard"),
                    _ => RedirectToPage("Index") 
                };
            }

            
            return Page();

        }
        public async Task<IActionResult> OnPostLogin(){
            try
            {
                if(!ModelState.IsValid){
                    throw new Exception("All data should be filled");
                }
                userData = await _context.employee.Where(u=>u.Email==userlogin.Email && u.Password==userlogin.Password).FirstOrDefaultAsync();
                if(userData==null){
                    TempData["err"]="Invalid Credentials";
                    throw new Exception("Invalid Credentials");
                }
                userDesignation = await _context.designation.FirstOrDefaultAsync(d=>d.DesignationId==userData.DesignationId);
                var designame = "";
                var desigid = "";
                var orgid = "";
                var planid = "";
                var orgdet = await _context.organization.Where(o=>o.OrgId == userData.OrgId).FirstOrDefaultAsync();
                if(userDesignation!=null){
                    designame = userDesignation.Name.ToString();
                    desigid = userData.DesignationId.ToString();
                    
                }
                if(orgdet!=null){
                    orgid = orgdet.OrgId.ToString();
                    planid = orgdet.PlanID.ToString();
                }
                    var claims = new List<Claim>{new Claim("empID",userData.EmployeeId.ToString()),
                                                    new Claim("OrgID",orgid),
                                                    new Claim("DesigName",designame),
                                                    new Claim("DesigID",desigid),
                                                    new Claim("PlanID",planid)};

                    
                    var claimsIdentity = new ClaimsIdentity(claims,CookieAuthenticationDefaults.AuthenticationScheme);
                    var authProperties= new AuthenticationProperties{IsPersistent=false};

                    HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,new ClaimsPrincipal(claimsIdentity),authProperties);

                if(userDesignation==null){
                    return RedirectToPage("Index");
                }
                else if(userDesignation.Name=="HR"){
                    
                    return RedirectToPage("HR/Dashboard");
                }
                else if(userDesignation.Name=="MANAGER"){
                    return RedirectToPage("Manager/Dashboard");
                }else if(userDesignation.Name=="PROJECT_MANAGER"){
                    return RedirectToPage("ProjectManager/Dashboard");
                }else if(userDesignation.Name=="TEAM_LEAD"){
                    return RedirectToPage("TeamLead/Board");
                }else if(userDesignation.Name=="TEAM_MEMBERS"){
                    return RedirectToPage("TeamMember/Dashboard");
                }
                return Page();
            }
            catch (Exception e)
            {
                
               Console.WriteLine($"login error=> {e.Message}");
               return Page();
            }
        }
    }
}