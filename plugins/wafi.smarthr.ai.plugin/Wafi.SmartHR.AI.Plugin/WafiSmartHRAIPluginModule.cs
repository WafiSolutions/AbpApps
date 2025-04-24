using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Account;
using Volo.Abp.FeatureManagement;
using Volo.Abp.Identity;
using Volo.Abp.Modularity;
using Volo.Abp.PermissionManagement;
using Volo.Abp.SettingManagement;
using Volo.Abp.TenantManagement;
using Wafi.Abp.OpenAISemanticKernel;
using Wafi.SmartHR.AI.Plugin.Employees;

namespace Wafi.SmartHR.AI.Plugin;

[DependsOn(
    typeof(WafiOpenAISemanticKernelModule),
    typeof(SmartHRApplicationContractsModule)
)]
public class WafiSmartHRAIPluginModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddTransient<IWafiPluginProvider, EmployeePluginProvider>();
        context.Services.AddTransient<IWafiPluginProvider, LeaveRecordPluginProvider>();
    }
}