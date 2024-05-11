using Microsoft.EntityFrameworkCore;
using RabbitMQ.API.Entities;

namespace RabbitMQ.API
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
            //startup tarafında options doldurcağımızdan dolayı base'e options verdik
        }

        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<OrderStatus> OrderStatuses { get; set; }
        public DbSet<Product> Products { get; set; }
    }
}
