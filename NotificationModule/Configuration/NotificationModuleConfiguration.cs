using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace NotificationModule.Configuration
{
    public static class NotificationModuleConfiguration
    {
        /// <summary>
        ///     cd.\NotificationModule
        ///     
        ///     dotnet ef --startup-project ..\ProjectNomad\Server\ migrations add NameOfMigration -c NotificationModuleDbContext
        /// note: you are in migration and context library project and are referring to startup project
        /// 
        ///     dotnet ef --startup-project ..\ProjectNomad\Server\ database update -c NotificationModuleDbContext
        /// </summary>
        public static void DbContextConfiguration(IServiceCollection services, string connectionStrings)
        {
            services.AddDbContextFactory<NotificationModuleDbContext>(options =>
                options.UseSqlServer(connectionStrings,
                x => x.MigrationsAssembly("NotificationModule")));
        }

        public static void RegisterIoC(IServiceCollection services)
        {
            services.AddScoped<INotificationModule, NotificationModule>();
        }
    }
}
