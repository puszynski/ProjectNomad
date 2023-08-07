using GameModule.Entities;
using Microsoft.EntityFrameworkCore;

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

        protected override void OnModelCreating(ModelBuilder modelBuilder) 
            => modelBuilder.HasDefaultSchema("GameModule");
    }
}
