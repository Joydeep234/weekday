using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using weekday.Data.Context;
using weekday.Data.Entity;
using weekday.Middleware;

namespace weekday.Components
{
    public class NavbarViewComponent :ViewComponent
    {
        private readonly AppDbcontext _context;
        public NavbarViewComponent(AppDbcontext dbcontext){
            _context = dbcontext;
        }

        public List<Features> feature { get; set; } = new List<Features>();
        public async Task<IViewComponentResult> InvokeAsync(){
            var userRole = UserClaimsPrincipal?.FindFirst("DesigName")?.Value;
            feature = await (from f in _context.feature
                            join d in _context.designation on f.designationId equals d.DesignationId
                            where d.Name == userRole
                            select f).ToListAsync();
            if(feature.Count<1)throw new CustomExceptionClass("Feature not available");

            return View(feature);
        }
    }
}