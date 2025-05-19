using System.Threading.Tasks;

namespace Wafi.SmartHR.Data;

public interface ISmartHRDbSchemaMigrator
{
    Task MigrateAsync();
}
