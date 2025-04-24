using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wafi.Abp.OpenAISemanticKernel;

namespace Wafi.SmartHR.AI.Plugin.Employees
{
    public class EmployeePluginProvider : SemanticPluginProviderBase<EmployeePlugin>
    {
        public EmployeePluginProvider(EmployeePlugin plugin) : base(plugin)
        {
        }

        public override string Name => "Employee";
    }
}
