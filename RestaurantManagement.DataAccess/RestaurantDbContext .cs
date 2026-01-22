using Microsoft.EntityFrameworkCore;
using RestaurantManagement.DataAccess.Models;
using RestaurantManagement.Models.Common.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RestaurantManagement.DataAccess
{
    public class RestaurantDbContext : DbContext
    {
        public RestaurantDbContext(DbContextOptions<RestaurantDbContext> options) : base(options) { }

        public DbSet<User> Users => Set<User>();
        public DbSet<Customer> Customers => Set<Customer>();
        public DbSet<MenuItem> MenuItems => Set<MenuItem>();
        public DbSet<Order> Orders => Set<Order>();
        public DbSet<OrderItem> OrderItems => Set<OrderItem>();
        public DbSet<Bill> Bills => Set<Bill>();
        public DbSet<Review> Reviews => Set<Review>();
        public DbSet<RestaurantSettings> RestaurantSettings => Set<RestaurantSettings>();
        public DbSet<Table> Tables => Set<Table>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            var adminId = Guid.NewGuid();

            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id = adminId,
                    Name = "Admin",
                    Email = "admin@gmail.com",
                    MobileNumber = "9999999999",
                    Role = UserRole.Admin,
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow,
                    Password = BCrypt.Net.BCrypt.HashPassword("Chandrapal@123")
                }
            );
            Guid Id = Guid.NewGuid();
            modelBuilder.Entity<RestaurantSettings>().HasData(
                new RestaurantSettings
                {
                    Id = Id,
                    TaxPercent = 10,
                    DiscountPercent = 10,
                    UpdatedAt = new DateTime(2026, 01, 01),
                    UpdatedByAdminId = adminId
                }
            );
    }
    }
}
