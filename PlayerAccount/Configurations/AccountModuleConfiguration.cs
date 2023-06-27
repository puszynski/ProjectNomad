using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace AccountModule.Configuration
{
    /// <summary>
    ///     cd.\PlayerAccount
    ///     
    ///     dotnet ef --startup-project ..\ProjectNomad\Server\ migrations add NameOfMigration -c AccountModuleDbContext
    /// note: you are in migration and context library project and are referring to startup project
    /// 
    ///     dotnet ef --startup-project ..\ProjectNomad\Server\ database update -c AccountModuleDbContext
    /// </summary>
    public static class AccountModuleConfiguration
    {
        public static void DbContextConfiguration(IServiceCollection services, string connectionStrings)
        {
            services.AddDbContextFactory<AccountModuleDbContext>(options => 
                options.UseSqlServer(connectionStrings, 
                x => x.MigrationsAssembly("AccountModule")));
        }

        public static void RegisterIoC(IServiceCollection services)
        {
            services.AddScoped<IAccountModule, AccountModule>();
        }
    }
}
