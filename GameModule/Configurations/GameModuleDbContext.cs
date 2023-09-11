using GameModule.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata;

namespace GameModule.Configurations
{
    internal class GameModuleDbContext : DbContext
    {
        public GameModuleDbContext(DbContextOptions<GameModuleDbContext> options) : base(options)
        {
        }

        public DbSet<Tribe> Tribes { get; set; }
        public DbSet<HumanUnit> HumanUnits { get; set; }
        public DbSet<MapTile> MapTiles { get; set; }
        public DbSet<HumanUnitTask> HumanUnitTasks { get; set; }
        public DbSet<HumanUnitTaskOrder> HumanUnitTaskOrders { get; set; }
        public DbSet<TribeRelocation> TribeRelocations { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("GameModule");

            //modelBuilder.Entity<Tribe>()
            //    .HasOne(e => e.Relocation)
            //    .WithOne(e => e.Tribe)
            //    //.HasForeignKey<TribeRelocation>(e => e.TribeId) //??? https://learn.microsoft.com/en-us/ef/core/modeling/relationships/one-to-one
            //    .IsRequired(false);
        }
    }
}
