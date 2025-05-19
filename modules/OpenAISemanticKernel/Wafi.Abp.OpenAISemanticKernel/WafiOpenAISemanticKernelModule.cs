using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.SemanticKernel;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Modularity;
using Wafi.Abp.OpenAISemanticKernel.Plugins;
using Wafi.Abp.OpenAISemanticKernel.Services;

namespace Wafi.Abp.OpenAISemanticKernel;

[DependsOn(typeof(AbpAspNetCoreMvcModule))]
public class WafiOpenAISemanticKernelModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        ConfigureOpenAISemanticKernelOptions(context);
        ConfigureAutoAPIControllers();

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

    private void ConfigureAutoAPIControllers()
    {
        Configure<AbpAspNetCoreMvcOptions>(options =>
        {
            options.ConventionalControllers.Create(typeof(WafiOpenAISemanticKernelModule).Assembly, opts =>
            {
                opts.RootPath = "OpenAISemanticKernel";
            });
        });
    }

    private void ConfigureOpenAISemanticKernelOptions(ServiceConfigurationContext context)
    {
        var configuration = context.Services.GetConfiguration();

        Configure<WafiOpenAISemanticKernelOptions>(options =>
        {
            options.ModelId = configuration.GetValue<string>("SemanticKernel:OpenAI:ModelId");
            options.ApiKey = configuration.GetValue<string>("SemanticKernel:OpenAI:ApiKey");
        });

    }
}
