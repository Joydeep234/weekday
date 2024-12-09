using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using weekday.Data.Context;
using weekday.Data.Entity;

namespace weekday.Pages.Manager
{
    [Authorize (Policy ="MANAGER")]
    public class memberdetails{
        public int empid{ get; set; }
        public string firstname{ get; set; }
        public string lastname{ get; set; }
        public string email { get; set; }
        public string designation { get; set; }
        public string status { get; set; }


    }
    public class AllTeamShow : PageModel
    {
        private readonly AppDbcontext _context;
        private readonly ILogger<AllTeamShow> _logger;
        
        public AllTeamShow(ILogger<AllTeamShow> logger,AppDbcontext dbcontext)
        {
            _logger = logger;
            _context = dbcontext;
        }
        public List<memberdetails> memdetails { get; set; } = new List<memberdetails>();
        public int projectID { get; set; }
        public int newteamID { get; set; }
        public async Task OnGet(int projID,int teamID)
        {
             projectID = projID;
            newteamID = teamID;
            
                try
                {
                    memdetails = await (from tm in _context.teamMembers
                                        join e in _context.employee
                                        on tm.MemberId equals e.EmployeeId
                                        join d in _context.designation on e.DesignationId equals d.DesignationId
                                        where tm.TeamId == teamID
                                        select new memberdetails{
                                            empid = e.EmployeeId,
                                            firstname = e.FirstName,
                                            lastname = e.LastName,
                                            email = e.Email,
                                            designation = d.Name,
                                            status = tm.status
                                        }).ToListAsync();
                    
                }
                catch (System.Exception e)
                {
                    System.Console.WriteLine($"member det error {e.Message}");
                }
        }

        public async Task<IActionResult> OnPostRemoveMember(int memberID,int projid,int teamid){
            try
            {
                var member = await _context.teamMembers.Where(t=>t.MemberId == memberID && t.TeamId ==teamid).FirstOrDefaultAsync();
                if(member==null)throw new Exception("member is empty");
                _context.teamMembers.Remove(member);
                await _context.SaveChangesAsync();
                 return RedirectToPage("../Manager/AddManagers",new {projID = projid,teamID=teamid});
            }
            catch (System.Exception e)
                {
                    System.Console.WriteLine($"member det error {e.Message}");
                     return Page();
                }
           
        }
    }
}