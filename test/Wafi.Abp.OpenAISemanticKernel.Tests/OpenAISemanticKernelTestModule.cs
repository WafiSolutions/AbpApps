using Microsoft.Extensions.DependencyInjection;
using System;
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
        // Override configuration with test values
        context.Services.Configure<WafiOpenAISemanticKernelOptions>(options =>
        {
            options.ModelId = "test-model";
            options.ApiKey = "test-api-key";
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
