using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FCRabbitMQ.ExcelCreate.Models
{
    public class AppDbContext : IdentityDbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        public DbSet<UserFile> UserFiles { get; set; }
        public DbSet<UserDepartment> UserDepartments { get; set; }
        public DbSet<Department> Departments { get; set; }
    }
}
