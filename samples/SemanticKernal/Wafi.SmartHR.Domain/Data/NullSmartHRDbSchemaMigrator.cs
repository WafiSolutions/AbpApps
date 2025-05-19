using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace Wafi.SmartHR.Data;

/* This is used if database provider does't define
 * ISmartHRDbSchemaMigrator implementation.
 */
public class NullSmartHRDbSchemaMigrator : ISmartHRDbSchemaMigrator, ITransientDependency
{
    public Task MigrateAsync()
    {
        return Task.CompletedTask;
    }
}
