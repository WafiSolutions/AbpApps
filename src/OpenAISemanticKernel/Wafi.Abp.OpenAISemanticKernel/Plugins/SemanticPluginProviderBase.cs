namespace Wafi.Abp.OpenAISemanticKernel.Plugins;

public abstract class SemanticPluginProviderBase<TPlugin> : IWafiPluginProvider
{
    protected readonly TPlugin PluginInstance;

    protected SemanticPluginProviderBase(TPlugin pluginInstance)
    {
        PluginInstance = pluginInstance;
    }

    public virtual string Name => typeof(TPlugin).Name;

    public virtual WafiKernelPlugin GetPlugin()
    {
        return new WafiKernelPlugin
        {
            Instance = PluginInstance,
            Name = Name
        };
    }
}
