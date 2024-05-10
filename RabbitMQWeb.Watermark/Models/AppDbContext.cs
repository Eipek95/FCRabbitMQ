using Microsoft.EntityFrameworkCore;

namespace RabbitMQWeb.Watermark.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
            //startup tarafında options doldurcağımızdan dolayı base'e options verdik
        }

        public DbSet<Product> Products { get; set; }
    }
}
