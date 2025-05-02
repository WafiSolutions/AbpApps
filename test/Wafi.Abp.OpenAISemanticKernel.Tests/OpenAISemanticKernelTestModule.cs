using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp;
using Volo.Abp.Autofac;
using Volo.Abp.Modularity;

namespace Wafi.Abp.OpenAISemanticKernel;

[DependsOn(
    typeof(WafiOpenAISemanticKernelModule),
    typeof(AbpTestBaseModule),
    typeof(AbpAutofacModule)
)]
public class OpenAISemanticKernelTestModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        var configuration = context.Services.GetConfiguration();

        // Configure OpenAI settings from appsettings.json
        context.Services.Configure<WafiOpenAISemanticKernelOptions>(options =>
        {
            options.ModelId = configuration.GetValue<string>("SemanticKernel:OpenAI:ModelId");
            options.ApiKey = configuration.GetValue<string>("SemanticKernel:OpenAI:ApiKey");
        });
    }

    public override void OnApplicationInitialization(ApplicationInitializationContext context)
    {
        SeedTestData(context);
    }

    private static void SeedTestData(ApplicationInitializationContext context)
    {
        // Add any test data seeding here if needed
    }
}
