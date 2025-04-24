using Microsoft.SemanticKernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wafi.Abp.OpenAISemanticKernel
{
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
}
