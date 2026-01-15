using Microsoft.EntityFrameworkCore;
using RestaurantManagement.DataAccess.Models;

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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>()
                .HasIndex(x => x.Email)
                .IsUnique();


            modelBuilder.Entity<Order>()
                .HasOne(o => o.Customer)
                .WithMany(c => c.Orders)
                .HasForeignKey(o => o.CustomerId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Order>()
                .HasOne(o => o.Waiter)
                .WithMany(u => u.OrdersTaken)
                .HasForeignKey(o => o.WaiterId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<OrderItem>()
                .HasOne(oi => oi.Order)
                .WithMany(o => o.Items)
                .HasForeignKey(oi => oi.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<OrderItem>()
                .HasOne(oi => oi.MenuItem)
                .WithMany(mi => mi.OrderItems)
                .HasForeignKey(oi => oi.MenuItemId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Bill>()
                .HasOne(b => b.Order)
                .WithOne(o => o.Bill)
                .HasForeignKey<Bill>(b => b.OrderId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Bill>()
                .HasOne(b => b.Customer)
                .WithMany(c => c.Bills)
                .HasForeignKey(b => b.CustomerId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Bill>()
                .HasOne(b => b.GeneratedByWaiter)
                .WithMany(u => u.BillsGenerated)
                .HasForeignKey(b => b.GeneratedByWaiterId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Review>()
                .HasOne(r => r.Customer)
                .WithMany(c => c.Reviews)
                .HasForeignKey(r => r.CustomerId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<RestaurantSettings>()
                .HasOne(s => s.UpdatedByAdmin)
                .WithMany()
                .HasForeignKey(s => s.UpdatedByAdminId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
