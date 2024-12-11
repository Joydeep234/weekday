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
using weekday.Middleware;
using weekday.Models;

namespace weekday.Pages
{
    [Authorize]
    public class Account : PageModel
    {
        private readonly IWebHostEnvironment _enviroment;
        private readonly AppDbcontext _context;
        private readonly ILogger<Account> _logger;

        public Account(ILogger<Account> logger,AppDbcontext context,IWebHostEnvironment _env)
        {
            _logger = logger;
            _context = context;
            _enviroment = _env;
        }
        public int employeeId { get; set; }
        public Employee employeedetails { get; set; } = new Employee();

        [BindProperty]
        public AccountEditModelClass accountmodel { get; set; }
        [BindProperty]
        public string OldPassword { get; set; }
        [BindProperty]
        public string Newpassword { get; set; }
        public async Task OnGet()
        {
            employeeId = Convert.ToInt32(User.FindFirst("empID")?.Value);
            if(employeeId == 0)throw new Exception("employeee id not found");
            employeedetails = await _context.employee.Where(e=>e.EmployeeId == employeeId).FirstOrDefaultAsync();
            if(employeedetails == null)throw new Exception("Employee details not found");
        }

        public async Task<IActionResult> OnPostHandlePersonalDetails(){
            
            employeeId = Convert.ToInt32(User.FindFirst("empID")?.Value);
            if(employeeId == 0)throw new Exception("employeee id not found");
            var empdet = await _context.employee.Where(e=>e.EmployeeId == employeeId).FirstOrDefaultAsync();
            if(employeedetails == null)throw new Exception("Employee details not found");

            empdet.FirstName = accountmodel.firstName!=null?accountmodel.firstName:empdet.FirstName;
            empdet.LastName = accountmodel.lastName!=null?accountmodel.lastName:empdet.LastName;
           
            if (accountmodel.image != null)
        {
            string oldFilePath = Path.Combine(_enviroment.WebRootPath, "uploads", empdet.ImageURL);

            
            if (System.IO.File.Exists(oldFilePath))
            {
                System.IO.File.Delete(oldFilePath);
            }

            
            var newFileName = Guid.NewGuid().ToString() + Path.GetExtension(accountmodel.image.FileName);
            var newFilePath = Path.Combine(_enviroment.WebRootPath, "uploads", newFileName);

            using (var fileStream = new FileStream(newFilePath, FileMode.Create))
            {
                await accountmodel.image.CopyToAsync(fileStream);
                fileStream.Flush(); 
            }

            empdet.ImageURL = newFileName;
        }
                

            await _context.SaveChangesAsync();

            return RedirectToPage("Account");
            
           
        }

        public async Task<IActionResult> OnPostHandlePassword(){
             employeeId = Convert.ToInt32(User.FindFirst("empID")?.Value);
            if(employeeId == 0)throw new Exception("employeee id not found");
            var empdet = await _context.employee.Where(e=>e.EmployeeId == employeeId).FirstOrDefaultAsync();
            if(employeedetails == null)throw new Exception("Employee details not found");

            if(OldPassword == empdet.Password){
                empdet.Password=Newpassword;
            }else{
                throw new CustomExceptionClass("Old Password not correct");
            }

            await _context.SaveChangesAsync();

            return RedirectToPage("Account");
        }
    }
}