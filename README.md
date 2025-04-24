# Wafi.SmartHR - Semantic Kernel Integration

## Overview

This project demonstrates the integration of Microsoft's Semantic Kernel SDK with ABP.io framework. It provides a wrapper implementation that makes it easy to use Semantic Kernel in ABP.io applications, allowing you to create AI-powered plugins for your application services.

## Architecture

The solution consists of two main modules:

1. **WafiOpenAISemanticKernelModule**: Core module that provides Semantic Kernel integration
2. **WafiSmartHRAIPluginModule**: Sample implementation module showing how to create AI plugins

### Core Components

#### 1. WafiOpenAISemanticKernelModule

This module provides the foundation for Semantic Kernel integration:

```csharp
public class WafiOpenAISemanticKernelModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.Configure<WafiOpenAISemanticKernelOptions>(options =>
        {
            /* Set via appsettings.json or externally */
        });

        context.Services.AddSingleton(serviceProvider =>
        {
            var options = serviceProvider.GetRequiredService<IOptions<WafiOpenAISemanticKernelOptions>>().Value;
            var kernelBuilder = Kernel.CreateBuilder()
                .AddOpenAIChatCompletion(modelId: options.ModelId, apiKey: options.ApiKey);
            
            // Register plugins
            var pluginProviders = serviceProvider.GetServices<IWafiPluginProvider>();
            foreach (var provider in pluginProviders)
            {
                var plugin = provider.GetPlugin();
                var skPlugin = KernelPluginFactory.CreateFromObject(
                    plugin.Instance,
                    plugin.Name
                );
                kernelBuilder.Plugins.Add(skPlugin);
            }

            return kernelBuilder.Build();
        });
    }
}
```

#### 2. Base Classes and Interfaces

- `SemanticPluginProviderBase<TPlugin>`: Base class for all application plugins
- `IWafiPluginProvider`: Interface for plugin providers

### Sample Implementation

The solution includes two sample plugins:

1. **EmployeePlugin**: For employee-related operations
2. **LeaveRecordPlugin**: For leave record management

Example of a plugin implementation:

```csharp
public class EmployeePlugin : ApplicationService, ITransientDependency
{
    private readonly IEmployeeAppService _employeeService;
    private readonly IAuthorizationService _authorizationService;

    public EmployeePlugin(IEmployeeAppService employeeService, IAuthorizationService authorizationService) 
    {
        _employeeService = employeeService;
        _authorizationService = authorizationService;
    }

    [KernelFunction, Description("Get employee list")]
    public async Task<string> GetEmployeesAsync()
    {
        if (!await _authorizationService.IsGrantedAsync(SmartHRPermissions.Employees.Default))
        {
            return "You are not authorized to access employee records";
        }

        var result = await _employeeService.GetListAsync();
        return JsonSerializer.Serialize(result);
    }
}
```

## Configuration

### Module Dependencies

To use the Semantic Kernel integration, add the following dependencies to your module:

```csharp
[DependsOn(
    typeof(SmartHRApplicationContractsModule),
    typeof(AbpPermissionManagementHttpApiModule),
    typeof(AbpSettingManagementHttpApiModule),
    typeof(AbpAccountHttpApiModule),
    typeof(AbpIdentityHttpApiModule),
    typeof(AbpTenantManagementHttpApiModule),
    typeof(AbpFeatureManagementHttpApiModule),
    typeof(WafiOpenAISemanticKernelModule),
    typeof(WafiSmartHRAIPluginModule)
)]
public class SmartHRHttpApiModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        var configuration = context.Services.GetConfiguration();
        
        Configure<WafiOpenAISemanticKernelOptions>(options =>
        {
            options.ModelId = configuration.GetValue<string>("SemanticKernel:OpenAI:ModelId");
            options.ApiKey = configuration.GetValue<string>("SemanticKernel:OpenAI:ApiKey");
        });
    }
}
```

### AppSettings Configuration

Add the following configuration to your `appsettings.json`:

```json
{
  "SemanticKernel": {
    "OpenAI": {
      "ApiKey": "your-openai-key",
      "ModelId": "gpt-4"
    }
  }
}
```

## Usage

1. Create a new plugin class that inherits from `ApplicationService` and implements `ITransientDependency`
2. Create a provider class that inherits from `SemanticPluginProviderBase<TPlugin>`
3. Register your plugin provider in your module's `ConfigureServices` method
4. Use the `[KernelFunction]` attribute to expose methods to the Semantic Kernel

## Security

The implementation includes built-in authorization checks using ABP's permission system. Each plugin method should check for appropriate permissions before executing sensitive operations.

## Contributing

Contributions are welcome! Please feel free to submit a Pull Request.

## License

This project is licensed under the MIT License - see the LICENSE file for details.
