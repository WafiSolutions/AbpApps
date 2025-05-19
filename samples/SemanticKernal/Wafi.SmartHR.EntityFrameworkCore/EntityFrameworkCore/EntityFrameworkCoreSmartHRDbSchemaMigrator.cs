using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Wafi.SmartHR.Data;
using Volo.Abp.DependencyInjection;

namespace Wafi.SmartHR.EntityFrameworkCore;

public class EntityFrameworkCoreSmartHRDbSchemaMigrator
    : ISmartHRDbSchemaMigrator, ITransientDependency
{
    private readonly IServiceProvider _serviceProvider;

    public EntityFrameworkCoreSmartHRDbSchemaMigrator(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task MigrateAsync()
    {
        /* We intentionally resolving the SmartHRDbContext
         * from IServiceProvider (instead of directly injecting it)
         * to properly get the connection string of the current tenant in the
         * current scope.
         */

        await _serviceProvider
            .GetRequiredService<SmartHRDbContext>()
            .Database
            .MigrateAsync();
    }
}
