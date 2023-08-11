using GameModule.Logic;
using GameModule.Logic.GameLooperLogic;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace GameModule.Configurations
{
    public static class GameModuleConfiguration
    {
        /// <summary>
        /// to run migration
        ///     cd .\GameModule
        ///     dotnet ef --startup-project ..\ProjectNomad\Server\ migrations add AddMapTileIdToHumanUnitTask -c GameModuleDbContext
        ///     
        ///  note: you are in migration and context library project and are referring to startup project
        ///  plus specify context where multiple are detected by -c Name
        /// 
        /// others commands:
        ///     dotnet ef --startup-project ..\ProjectNomad\Server\ database update -c GameModuleDbContext
        ///     
        /// FOR THE FIRST TIME DB SHOULD BE CREATED:
        /// => install globally via powerShell => dotnet tool install --global dotnet-ef
        /// => then run database update.. 
        /// </summary>
        public static void DbContextConfiguration(IServiceCollection services, string connectionStrings)
        {
            services.AddDbContextFactory<GameModuleDbContext>(options =>
                options.UseSqlServer(connectionStrings,
                x => x.MigrationsAssembly("GameModule")));
        }

        public static void RegisterIoC(IServiceCollection services)
        {
            services.AddScoped<NewTribeLocalizationInitializer>();
            services.AddScoped<IGameModule, GameModule>();
            services.AddScoped<GameLOOPER>();
            services.AddScoped<MapService>();
            services.AddScoped<HumanUnitTaskConsumer>();
        }
    }
}
