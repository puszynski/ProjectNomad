using GameModule.DataBaseModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameModule.Configurations
{
    internal class GameModuleDbContext : DbContext
    {
        public GameModuleDbContext(DbContextOptions<GameModuleDbContext> options) : base(options)
        {
        }

        public DbSet<Tribe> Tribes { get; set; }
        public DbSet<HumanUnit> HumanUnits { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("GameModule");
            //base.OnModelCreating(modelBuilder);
        }
    }
}
