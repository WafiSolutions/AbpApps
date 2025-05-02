namespace Wafi.Abp.OpenAISemanticKernel.Plugins;

public interface IWafiPluginProvider
{
    WafiKernelPlugin GetPlugin();
}
