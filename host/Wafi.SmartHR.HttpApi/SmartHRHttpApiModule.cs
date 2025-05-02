using Localization.Resources.AbpUi;
using Wafi.SmartHR.Localization;
using Volo.Abp.Account;
using Volo.Abp.SettingManagement;
using Volo.Abp.FeatureManagement;
using Volo.Abp.Identity;
using Volo.Abp.Modularity;
using Volo.Abp.PermissionManagement.HttpApi;
using Volo.Abp.Localization;
using Volo.Abp.TenantManagement;
using Wafi.Abp.OpenAISemanticKernel;
using Wafi.SmartHR.AI.Plugin;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;

namespace Wafi.SmartHR;

 [DependsOn(
    typeof(SmartHRApplicationContractsModule),
    typeof(AbpPermissionManagementHttpApiModule),
    typeof(AbpSettingManagementHttpApiModule),
    typeof(AbpAccountHttpApiModule),
    typeof(AbpIdentityHttpApiModule),
    typeof(AbpTenantManagementHttpApiModule),
    typeof(AbpFeatureManagementHttpApiModule),
    typeof(WafiOpenAISemanticKernelModule),
    typeof(WafiSmartHRAIPluginModule)
    )]
public class SmartHRHttpApiModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        ConfigureLocalization();

        var configuration = context.Services.GetConfiguration();

        Configure<WafiOpenAISemanticKernelOptions>(options =>
        {
            options.ModelId = configuration.GetValue<string>("SemanticKernel:OpenAI:ModelId");
            options.ApiKey = configuration.GetValue<string>("SemanticKernel:OpenAI:ApiKey");
        });
    }

    private void ConfigureLocalization()
    {
        Configure<AbpLocalizationOptions>(options =>
        {
            options.Resources
                .Get<SmartHRResource>()
                .AddBaseTypes(
                    typeof(AbpUiResource)
                );
        });
    }
}
