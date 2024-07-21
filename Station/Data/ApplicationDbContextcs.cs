using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Station.Models;

namespace Station.Data
{
    public class ApplicationDbContext : IdentityDbContext<IdentityUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Item> Items { get; set; }
        public DbSet<Bill> Bills { get; set; }
        public DbSet<Meter> Meters { get; set; }
        public DbSet<Employee> Employees { get; set; }

        public DbSet<Reading> Readings { get; set; }
        public DbSet<Point> Points { get; set; }
        public DbSet<Subscriber> Subscribers { get; set; }
        public DbSet<Subscription> Subscriptions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<IdentityRole>().HasData(
                new IdentityRole()
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = "Admin",
                    NormalizedName = "admin",
                    ConcurrencyStamp = Guid.NewGuid().ToString(),
                },
                new IdentityRole()
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = "User",
                    NormalizedName = "user",
                    ConcurrencyStamp = Guid.NewGuid().ToString(),
                }

                );

            base.OnModelCreating(modelBuilder);

            // Configure your entity relationships and keys here
            modelBuilder.Entity<Meter>()
                .HasKey(m => m.Id);

            modelBuilder.Entity<Meter>()
                .HasOne(m => m.Subscriber)
                .WithOne(s => s.Meter)
                .HasForeignKey<Subscriber>(s => s.MeterId);

            modelBuilder.Entity<Item>()
                .HasMany(i => i.Subscriber)
                .WithOne(s => s.Item)
                .HasForeignKey(s => s.PointId);

            modelBuilder.Entity<Item>()
                .HasMany(i => i.Meter)
                .WithOne(m => m.Item)
                .HasForeignKey(m => m.ItemId)
                .IsRequired(false);
            modelBuilder.Entity<Subscriber>()
                .HasIndex(a => a.SubscriberNumber)
                .IsUnique();
                

        }
    }
}
