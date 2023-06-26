using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace GameModule.Configurations
{
    public static class GameModuleDbContextConfiguration
    {
        /// <summary>
        /// to run migration
        ///     cd.\GameModule
        ///     dotnet ef --startup-project ..\ProjectNomad\Server\ migrations add InitForGameModule -c GameModuleDbContext
        /// note: you are in migration and context library project and are referring to startup project
        ///  plus specify context where multiple are detected by -c Name
        /// 
        /// others commends:
        ///     dotnet ef --startup-project ..\ProjectNomad\Server\ database update
        ///     
        /// 
        /// 
        /// todo ?? => add-migration NAME_OF_MIGRATION -ConfigurationTypeName FULLY_QUALIFIED_NAME_OF_CONFIGURATION_CLASS
        /// </summary>
        public static void DbContextConfiguration(IServiceCollection services, string connectionStrings)
        {
            services.AddDbContextFactory<GameModuleDbContext>(options =>
                options.UseSqlServer(connectionStrings,
                x => x.MigrationsAssembly("GameModule")));
        }

        //internal sealed class GameModuleDbContextConfigurationConfiguration : DbMigrationsConfiguration<GameModuleDbContext>
        //{
        //    public ConfigurationA()
        //    {
        //        AutomaticMigrationsEnabled = false;
        //        MigrationsDirectory = @"Migrations\ModelA";
        //    }
        //}
    }
}
