using System;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Wafi.SmartHR.EntityFrameworkCore;

/* This class is needed for EF Core console commands
 * (like Add-Migration and Update-Database commands) */
public class SmartHRDbContextFactory : IDesignTimeDbContextFactory<SmartHRDbContext>
{
    public SmartHRDbContext CreateDbContext(string[] args)
    {
        var configuration = BuildConfiguration();
        
        SmartHREfCoreEntityExtensionMappings.Configure();

        var builder = new DbContextOptionsBuilder<SmartHRDbContext>()
            .UseSqlServer(configuration.GetConnectionString("Default"));
        
        return new SmartHRDbContext(builder.Options);
    }

    private static IConfigurationRoot BuildConfiguration()
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../Wafi.SmartHR.DbMigrator/"))
            .AddJsonFile("appsettings.json", optional: false);

        return builder.Build();
    }
}
