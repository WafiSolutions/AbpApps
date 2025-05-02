using Wafi.Abp.OpenAISemanticKernel.Plugins;

namespace Wafi.SmartHR.AI.Plugin.LeaveRecords;

public class LeaveRecordPluginProvider : SemanticPluginProviderBase<LeaveRecordPlugin>
{
    public LeaveRecordPluginProvider(LeaveRecordPlugin plugin) : base(plugin)
    {
    }

    public override string Name => "LeaveRecord";
}
