using CourseStripe.Data.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CourseStripe.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public DbSet<ProductEntity> Products { get; set; }
        public DbSet<UserSubscriptionEntity> userSubscriptions { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserSubscriptionEntity>()
                .HasKey(a=> new { a.UserId, a.SubscriptionId });

            base.OnModelCreating(modelBuilder);
        }
    }
}
