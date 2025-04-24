using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wafi.Abp.OpenAISemanticKernel;
using Wafi.SmartHR.AI.Plugin.LeaveRecords;

namespace Wafi.SmartHR.AI.Plugin.Employees
{
    public class LeaveRecordPluginProvider : SemanticPluginProviderBase<LeaveRecordPlugin>
    {
        public LeaveRecordPluginProvider(LeaveRecordPlugin plugin) : base(plugin)
        {
        }

        public override string Name => "LeaveRecord";
    }
}
