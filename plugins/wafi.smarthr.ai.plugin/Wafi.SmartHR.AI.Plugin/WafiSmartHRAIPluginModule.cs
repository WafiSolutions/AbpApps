using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;
using Wafi.Abp.OpenAISemanticKernel;
using Wafi.SmartHR.AI.Plugin.Employees;
using Wafi.SmartHR.AI.Plugin.LeaveRecords;

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