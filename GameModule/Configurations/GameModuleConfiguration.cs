using GameModule.Logic;
using GameModule.Logic.GameLooperLogic;
using GameModule.Logic.GameLooperLogic.HourExecutorLogic;
using GameModule.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ProjectNomad.Shared;

namespace GameModule.Configurations
{
    public static class GameModuleConfiguration
    {
        /// <summary>
        /// to run migration
        ///     cd .\GameModule
        ///     dotnet ef --startup-project ..\ProjectNomad\Server\ migrations add RelocationOneToOne -c GameModuleDbContext
        ///     
        ///  note: you are in migration and context library project and are referring to startup project
        ///  plus specify context where multiple are detected by -c Name
        /// 
        /// others commands:
        ///     dotnet ef --startup-project ..\ProjectNomad\Server\ database update -c GameModuleDbContext
        ///     
        /// FOR THE FIRST TIME DB SHOULD BE CREATED:
        /// => install globally via powerShell => dotnet tool install --global dotnet-ef
        /// => run database update.. 
        /// </summary>
        public static void DbContextConfiguration(IServiceCollection services, string connectionStrings)
        {
            services.AddDbContextFactory<GameModuleDbContext>(options =>
                options.UseSqlServer(connectionStrings,
                x => x.MigrationsAssembly("GameModule")));
        }

        public static void RegisterIoC(IServiceCollection services)
        {
            services.AddScoped<IGameModule, GameModule>();
            services.AddScoped<NewTribeLocalizationInitializer>();
            services.AddScoped<IDateTimeProvider, DateTimeProvider>();

            services.AddScoped<ITribeRepository, TribeRepository>();
            services.AddScoped<IMapTileRepository, MapTileRepository>();
            services.AddScoped<IHumanUnitRepository, HumanUnitRepository>();
            services.AddScoped<IHumanUnitTaskRepository,  HumanUnitTaskRepository>();
            services.AddScoped<IHumanUnitTaskOrderRepository,  HumanUnitTaskOrderRepository>();

            services.AddScoped<GameLOOPER>();
            services.AddScoped<ISecundExecutor, SecundExecutor>();
            services.AddScoped<IMinuteExecutor, MinuteExecutor>();

            services.AddScoped<IHourExecutor, HourExecutor>();
            services.AddScoped<MapTileRegenerator>();
            services.AddScoped<BreedingApplicator>();

            services.AddScoped<IDayExecutor, DayExecutor>();

            services.AddScoped<MapService>();
            services.AddScoped<IHumanUnitTaskConsumer, HumanUnitTaskConsumer>();
            services.AddScoped<IHumanUnitAutoTaskScheduler, HumanUnitAutoTaskScheduler>();
            services.AddScoped<ITaskAssigner, TaskAssigner>();
            services.AddScoped<IHumansDeathApplicator, HumansDeathApplicator>();
            services.AddScoped<IGameOverApplicator, GameOverApplicator>();
        }
    }
}
