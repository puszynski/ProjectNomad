// Ignore Spelling: Sql

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace AccountModule.Configuration
{
    public static class AccountModuleDatabaseContextConfiguration
    {
        public static void ConfigureSqlServerDataContext(IServiceCollection services, string connectionStrings)
        {
            services.AddDbContextFactory<AccountContext>(options =>
                options.UseSqlServer(connectionStrings));
        }
    }
}
