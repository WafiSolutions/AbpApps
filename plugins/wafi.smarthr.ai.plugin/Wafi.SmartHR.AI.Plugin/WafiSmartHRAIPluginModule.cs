using Volo.Abp.Account;
using Volo.Abp.FeatureManagement;
using Volo.Abp.Identity;
using Volo.Abp.Modularity;
using Volo.Abp.PermissionManagement;
using Volo.Abp.SettingManagement;
using Volo.Abp.TenantManagement;
using Wafi.Abp.OpenAISemanticKernel;

namespace Wafi.SmartHR.AI.Plugin;

[DependsOn(
    typeof(WafiOpenAISemanticKernelModule),
    typeof(SmartHRApplicationContractsModule)
)]
public class WafiSmartHRAIPluginModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
    }
}