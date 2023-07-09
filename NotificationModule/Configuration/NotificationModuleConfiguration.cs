using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace NotificationModule.Configuration
{
    public static class NotificationModuleConfiguration
    {
        public static void DbContextConfiguration(IServiceCollection services, string connectionStrings)
        {
            services.AddDbContextFactory<NotificationModuleDbContext>(options =>
                options.UseSqlServer(connectionStrings,
                x => x.MigrationsAssembly("AccountModule")));
        }

        public static void RegisterIoC(IServiceCollection services)
        {
            services.AddScoped<INotificationModule, NotificationModule>();
        }
    }
}
