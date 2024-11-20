using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using weekday.Data.Entity;

namespace weekday.Data.Context
{
    public class AppDbcontext : DbContext
    {
        public AppDbcontext(DbContextOptions options) : base(options){}

        public DbSet<Designation> designation { get; set; }
        public DbSet<Employee> employee { get; set; }
        public DbSet<Organization> organization { get; set; }
        public DbSet<Project> project { get; set; }
        public DbSet<ProjectTask> projecttask { get; set; }
        public DbSet<TeamMembers> teamMembers { get; set; }
        public DbSet<Team> team { get; set; }
    }
}