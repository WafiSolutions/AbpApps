using Wafi.SmartHR.EntityFrameworkCore;
using Volo.Abp.Autofac;
using Volo.Abp.Modularity;

namespace Wafi.SmartHR.DbMigrator;

[DependsOn(
    typeof(AbpAutofacModule),
    typeof(SmartHREntityFrameworkCoreModule),
    typeof(SmartHRApplicationContractsModule)
)]
public class SmartHRDbMigratorModule : AbpModule
{
}
