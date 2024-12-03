using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.RazorPages;
using weekday.Data.Context;
using weekday.Data.Entity;
using weekday.Models.ProjectManagerModel;

namespace weekday.Pages
{
    public class SignUpModel : PageModel
    {
        public readonly AppDbcontext _context;
        private readonly IWebHostEnvironment _environment;

        public SignUpModel(AppDbcontext context,IWebHostEnvironment  environment)
        {
            _context = context;
            _environment = environment;
        }

        

        [BindProperty]
        public SignUp signUp { get; set; }
        public void OnGet()
        {
        }
        public async Task<IActionResult> OnPostAsync()
        {
            if (signUp.ImageURL != null)
            {
                string filename = Guid.NewGuid().ToString() + Path.GetExtension(signUp.ImageURL.FileName);
                string filepath = Path.Combine(_environment.WebRootPath, "Uploads", filename);
                Directory.CreateDirectory(Path.GetDirectoryName(filepath)!);
                var newEmployee = new Employee
                {
                    FirstName = signUp.FirstName,
                    LastName = signUp.LastName,
                    Email = signUp.Email,
                    Password = signUp.Password,
                    Status = "Active",
                    ImageURL = filename,

                };
                _context.employee.Add(newEmployee);
            }
            else
            {
                var newEmployee = new Employee
                {
                    FirstName = signUp.FirstName,
                    LastName = signUp.LastName,
                    Email = signUp.Email,
                    Password = signUp.Password,
                    Status = "Active",
                    ImageURL = "defaultImage.jpg"

                };
                _context.employee.Add(newEmployee);
            }
            

            

            
            await _context.SaveChangesAsync();

            return RedirectToPage("~/Login");
        }
        
    }
}
