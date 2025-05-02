using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.IO;
using Volo.Abp;
using Volo.Abp.Testing;

namespace Wafi.Abp.OpenAISemanticKernel;

public abstract class OpenAISemanticKernelTestBase : AbpIntegratedTest<OpenAISemanticKernelTestModule>
{
    protected override void SetAbpApplicationCreationOptions(AbpApplicationCreationOptions options)
    {
        options.UseAutofac();
        
        // Set the JSON configuration file path
        options.Services.ReplaceConfiguration(new ConfigurationBuilder()
            .AddJsonFile(Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json"), optional: true)
            .Build());
    }
}
