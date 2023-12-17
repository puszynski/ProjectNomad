using GameModule.Entities;
using Microsoft.EntityFrameworkCore;

namespace GameModule.Configurations
{
    internal class GameModuleDbContext : DbContext
    {
        public GameModuleDbContext(DbContextOptions<GameModuleDbContext> options) : base(options)
        {
        }

        public DbSet<MapTile> MapTiles { get; set; }
        public DbSet<WorldZoneParameter> WorldZoneParameter { get; set; }
        
        public DbSet<Tribe> Tribes { get; set; }

        public DbSet<Human> Humans { get; set; }
        public DbSet<Job> Jobs { get; set; }
        public DbSet<HumanTask> HumanTasks { get; set; }
        public DbSet<HumanTaskOrder> HumanTaskOrders { get; set; }
        public DbSet<TribeRelocation> TribeRelocations { get; set; }
        public DbSet<TribeStructure> TribeStructures { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("GameModule");

            modelBuilder.Entity<HumanTask>()
                .HasOne(x => x.Tribe)
                .WithMany(x => x.HumanTasks)
                .HasForeignKey(x => x.TribeId)
                .OnDelete(DeleteBehavior.ClientCascade);
            /// <summary>
            /// Error Number:1785,State:0,Class:16
            /// Introducing FOREIGN KEY constraint 'FK_HumanTasks_Tribes_TribeId' on table 'HumanTasks' may cause cycles or multiple cascade paths.Specify ON DELETE NO ACTION or ON UPDATE NO ACTION, or modify other FOREIGN KEY constraints.
            /// Could not create constraint or index.See previous errors.
            /// https://learn.microsoft.com/en-us/ef/core/saving/cascade-delete
            /// </summary>
        }
    }
}
