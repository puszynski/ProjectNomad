using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace AccountModule.Configuration
{
    /// <summary>
    /// to run migration
    ///     cd.\PlayerAccount
    ///     dotnet ef --startup-project ..\ProjectNomad\Server\ migrations add NameOfMigration
    /// note: you are in migration and context library project and are referring to startup project
    /// 
    /// others commends:
    ///     dotnet ef --startup-project ..\ProjectNomad\Server\ database update
    /// </summary>
    public static class AccountModuleDbContextConfiguration
    {
        public static void DbContextConfiguration(IServiceCollection services, string connectionStrings)
        {
            services.AddDbContextFactory<AccountContext>(options => 
                options.UseSqlServer(connectionStrings, 
                x => x.MigrationsAssembly("AccountModule")));
        }
    }
}
