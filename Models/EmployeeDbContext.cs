using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace TechTestApp.Models
{
    public class EmployeeDbContext : IdentityDbContext<Employee>
    {
        
            public EmployeeDbContext(DbContextOptions<EmployeeDbContext> options) : base(options)
            {
            }

            // Define your DbSet properties here for each entity you want to persist in the database
            // For example:
            // public DbSet<SomeEntity> SomeEntities { get; set; }

        public DbSet<Employee> Employees { get; set; } 
        
    }
}
