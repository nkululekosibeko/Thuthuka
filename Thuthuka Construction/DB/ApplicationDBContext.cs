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
        public DbSet<Quotation> quotations { get; set; }
        public DbSet<CustomerProject> customerProjects { get; set; }
        public DbSet<ApplicationUser> applicationUsers { get; set; }
        public DbSet<Progress> progresses { get; set; }
        public DbSet<Resource> resources { get; set; }
        public DbSet<QuotationResource> quotationResources { get; set; }

    }
}
