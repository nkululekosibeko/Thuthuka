using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Thuthuka_Construction.Models;

namespace Thuthuka_Construction.DB
{
    public class ApplicationDBContext : IdentityDbContext<IdentityUser>
    {
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base(options)
        {

        }

        public DbSet<ProjectType> projectTypes { get; set; }
        public DbSet<Project> projects { get; set; }
        public DbSet<Quatation> quatations { get; set; }
        public DbSet<CustomerRequirements> customerRequirements { get; set; }
        public DbSet<ApplicationUser> applicationUsers { get; set; }
    }
}
