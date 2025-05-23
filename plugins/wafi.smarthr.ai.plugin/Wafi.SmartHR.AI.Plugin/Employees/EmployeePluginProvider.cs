﻿using Wafi.Abp.OpenAISemanticKernel.Plugins;

namespace Wafi.SmartHR.AI.Plugin.Employees;

public class EmployeePluginProvider : SemanticPluginProviderBase<EmployeePlugin>
{
    public EmployeePluginProvider(EmployeePlugin plugin) : base(plugin)
    {
    }

    public override string Name => "Employee";
}
