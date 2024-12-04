using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using weekday.Data.Context;
using weekday.Data.Entity;
using weekday.Middleware;
using weekday.Models;

namespace weekday.Pages.Manager
{
    public class AddEmployees : PageModel
    {
        private readonly AppDbcontext _context;
        private readonly IWebHostEnvironment _enviroment;
        private readonly ILogger<AddEmployees> _logger;

        public AddEmployees(ILogger<AddEmployees> logger,AppDbcontext dbcontext,IWebHostEnvironment _env)
        {
            _logger = logger;
            _context = dbcontext;
            _enviroment=_env;
        }
        
        [BindProperty]
        public EmployeesModelClass employeeSelection { get; set; }
        public List<SelectListItem> Designationlist { get; set; }
        public async Task OnGet()
        {
            Designationlist = await _context.designation.Select(p=> new SelectListItem{Text=p.Name,Value=p.DesignationId.ToString()}).ToListAsync();
        }

        public async Task<IActionResult> OnPostAddingEmployees(){
            if(!ModelState.IsValid){
                    
                    throw new CustomExceptionClass("feilds shoud not be empty");
                }
                bool checkemailExisted = await _context.employee.Where(e=>e.Email==employeeSelection.Email).AnyAsync();
                if(checkemailExisted){
                     
                     throw new CustomExceptionClass("Email Already Existed");
                }
                if(employeeSelection.ImageURL==null){
                   
                    throw new CustomExceptionClass("Image Not Existed");
                    }
                if(employeeSelection.Password!=employeeSelection.CPassword){
                   
                    throw new CustomExceptionClass("Password And Confirm Password Not Same");
                }
                var filename = Guid.NewGuid().ToString() + Path.GetExtension(employeeSelection.ImageURL.FileName);
                var filepath = Path.Combine(_enviroment.WebRootPath,"uploads",filename);
                var fileStream = new FileStream(filepath,FileMode.Create);
                await employeeSelection.ImageURL.CopyToAsync(fileStream);

                var  emp = new Employee{
                    FirstName= employeeSelection.FirstName,
                    LastName = employeeSelection.LastName,
                    Email = employeeSelection.Email,
                    DesignationId = employeeSelection.Designation,
                    Status = employeeSelection.Status,
                    ImageURL = filename,
                    Password = employeeSelection.Password,
                    OrgId = 2
                };

                await _context.employee.AddAsync(emp);
                await _context.SaveChangesAsync();

                return RedirectToPage("../Manager/AddEmployees");
        }
    }
  
}