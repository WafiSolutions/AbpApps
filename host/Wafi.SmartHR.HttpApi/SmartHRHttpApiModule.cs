using Localization.Resources.AbpUi;
using Volo.Abp.Account;
using Volo.Abp.FeatureManagement;
using Volo.Abp.Identity;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.PermissionManagement.HttpApi;
using Volo.Abp.SettingManagement;
using Volo.Abp.TenantManagement;
using Wafi.Abp.OpenAISemanticKernel;
using Wafi.SmartHR.AI.Plugin;
using Wafi.SmartHR.Localization;

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
