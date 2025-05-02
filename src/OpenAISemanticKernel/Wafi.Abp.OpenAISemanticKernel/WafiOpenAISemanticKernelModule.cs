using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.SemanticKernel;
using Volo.Abp.Modularity;
using Wafi.Abp.OpenAISemanticKernel.Plugins;
using Wafi.Abp.OpenAISemanticKernel.Services.Chat;
using Microsoft.Extensions.Configuration;

namespace Wafi.Abp.OpenAISemanticKernel;

public class WafiOpenAISemanticKernelModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {

        var configuration = context.Services.GetConfiguration();

        Configure<WafiOpenAISemanticKernelOptions>(options =>
        {
            options.ModelId = configuration.GetValue<string>("SemanticKernel:OpenAI:ModelId");
            options.ApiKey = configuration.GetValue<string>("SemanticKernel:OpenAI:ApiKey");
        });

        context.Services.AddSingleton(serviceProvider =>
        {
            var options = serviceProvider.GetRequiredService<IOptions<WafiOpenAISemanticKernelOptions>>().Value;

            var kernelBuilder = Kernel.CreateBuilder()
                .AddOpenAIChatCompletion(modelId: options.ModelId, apiKey: options.ApiKey);

            var pluginProviders = serviceProvider.GetServices<IWafiPluginProvider>();

            foreach (var provider in pluginProviders)
            {
                var plugin = provider.GetPlugin();

                // Convert your abstraction to SemanticKernel
                var skPlugin = KernelPluginFactory.CreateFromObject(
                    plugin.Instance,
                    plugin.Name
                );

                kernelBuilder.Plugins.Add(skPlugin);
            }

            return kernelBuilder.Build();
        });

        context.Services.AddSingleton<IWafiChatCompletionService, WafiChatCompletionService>();
    }
}