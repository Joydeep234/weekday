using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
        public void OnGet()
        {


        }

        public async Task<IActionResult> OnPostLogin(){
            try
            {
                if(!ModelState.IsValid){
                    throw new Exception("All data should be filled");
                }
                userData = await _context.employee.Where(u=>u.Email==userlogin.Email && u.Password==userlogin.Password).FirstOrDefaultAsync();
                if(userData==null)throw new Exception("Invalid Credentials");
                userDesignation = await _context.designation.FirstOrDefaultAsync(d=>d.DesignationId==userData.DesignationId);
                if(userDesignation==null)throw new Exception("UserDesignation not found");
                

                if(userDesignation.Name=="HR"){
                    return RedirectToPage("HR/Dashboard");
                }
                else if(userDesignation.Name=="MANAGER"){
                    return RedirectToPage("Manager/Dashboard");
                }else if(userDesignation.Name=="PROJECT_MANAGER"){
                    return RedirectToPage("ProjectManager/Dashboard");
                }else if(userDesignation.Name=="TEAM_LEAD"){
                    return RedirectToPage("TeamLead/Dashboard");
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