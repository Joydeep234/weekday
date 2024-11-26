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

namespace weekday.Pages.Manager
{
    public class AddManagers : PageModel
    {
        private readonly AppDbcontext _context;
        private readonly ILogger<AddManagers> _logger;

        public AddManagers(ILogger<AddManagers> logger,AppDbcontext dbcontext)
        {
            _logger = logger;
            _context = dbcontext;
        }

        public List<Employee> employeelist { get; set; } = new List<Employee>();
        public async Task OnGet()
        {
           try
           {
                employeelist = await (from e in _context.employee
                                        join d in _context.designation
                                        on e.DesignationId equals d.DesignationId
                                        where d.Name == "PROJECT_MANAGER" 
                                        select e).ToListAsync();
                    if(employeelist.Count<1)throw new Exception("Employee list is empty");
           }
           catch (Exception e)
           {
            Console.WriteLine($"Add Manager error {e.Message}");
           }
                                
        }
    }
}