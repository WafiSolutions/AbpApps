using System.Threading.Tasks;

namespace Workspace.Sample.Data;

public interface ISampleDbSchemaMigrator
{
    Task MigrateAsync();
}
