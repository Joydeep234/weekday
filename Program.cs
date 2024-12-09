using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using weekday.Data.Context;
using weekday.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddDbContext<AppDbcontext>(options=>{
    string connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    options.UseSqlServer(connectionString);
});

builder.Services.AddSession(options=>{
    options.IdleTimeout = TimeSpan.FromMinutes(60);
    options.Cookie.IsEssential = true;
    options.Cookie.HttpOnly = true;
});
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
.AddCookie(Options=>{
    Options.LoginPath = "/Login";   
    Options.AccessDeniedPath = "/AccessDeniedPage";
    Options.LogoutPath = "/Index";
});

builder.Services.AddAuthorization(options =>
    {
        options.AddPolicy("MANAGER", policy =>
        policy.RequireClaim("DesigName", "MANAGER"));
        options.AddPolicy("PROJECT_MANAGER", policy =>
        policy.RequireClaim("DesigName", "PROJECT MANAGER"));
        options.AddPolicy("TEAM_LEAD", policy =>
        policy.RequireClaim("DesigName", "TEAM_LEAD"));
        options.AddPolicy("TEAM_MEMBERS", policy =>
        policy.RequireClaim("DesigName", "TEAM_MEMBERS"));
        options.AddPolicy("HR", policy =>
        policy.RequireClaim("DesigName", "HR"));
         options.AddPolicy("NoDesignationOrManager", policy =>
            policy.RequireAssertion(context =>
            {
                var desigNameClaim = context.User.FindFirst(c => c.Type == "DesigName");
                var desigName = desigNameClaim?.Value;
                return string.IsNullOrEmpty(desigName) || desigName == "MANAGER";
            })
        );
    });

var app = builder.Build();


if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}
app.UseMiddleware<GlobalExceptionHandler>();
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseSession();
app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();

app.Run();
