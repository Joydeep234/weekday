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
        policy.RequireClaim("DesigID", "1"));
        options.AddPolicy("PROJECT_MANAGER", policy =>
        policy.RequireClaim("DesigID", "2"));
        options.AddPolicy("TEAM_LEAD", policy =>
        policy.RequireClaim("DesigID", "3"));
        options.AddPolicy("TEAM_MEMBERS", policy =>
        policy.RequireClaim("DesigID", "4"));
        options.AddPolicy("HR", policy =>
        policy.RequireClaim("DesigID", "5"));
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
