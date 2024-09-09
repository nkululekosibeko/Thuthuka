using Microsoft.EntityFrameworkCore;

namespace Thuthuka_Construction.DB
{
    public class ApplicationDBContext :DbContext 
    {
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base(options)
        {

        }
    }
}
