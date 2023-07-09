using Microsoft.EntityFrameworkCore;
using NotificationModule.Entities;

namespace NotificationModule.Configuration
{
    internal class NotificationModuleDbContext : DbContext
    {
        public NotificationModuleDbContext(DbContextOptions<NotificationModuleDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
            => modelBuilder.HasDefaultSchema("NotificationModule");

        public DbSet<TribeNotification> TribeNotifications { get; set; }
    }
}
