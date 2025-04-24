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

The solution includes two sample plugins with their providers:

#### 1. Employee Plugin Implementation

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

public class EmployeePluginProvider : SemanticPluginProviderBase<EmployeePlugin>
{
    public EmployeePluginProvider(EmployeePlugin plugin) : base(plugin)
    {
    }

    public override string Name => "Employee";
}
```

#### 2. Leave Record Plugin Implementation

```csharp
public class LeaveRecordPlugin : ApplicationService, ITransientDependency
{
    private readonly ILeaveRecordAppService _leaveRecordService;
    private readonly IAuthorizationService _authorizationService;

    public LeaveRecordPlugin(ILeaveRecordAppService leaveRecordService, IAuthorizationService authorizationService) 
    {
        _leaveRecordService = leaveRecordService;
        _authorizationService = authorizationService;
    }

    [KernelFunction, Description("Get leave record list")]
    public async Task<string> GetLeaveRecordsAsync()
    {
        if (!await _authorizationService.IsGrantedAsync(SmartHRPermissions.LeaveRecords.Default))
        {
            return "You are not authorized to access leave records";
        }

        var result = await _leaveRecordService.GetListAsync();
        return JsonSerializer.Serialize(result);
    }
}

public class LeaveRecordPluginProvider : SemanticPluginProviderBase<LeaveRecordPlugin>
{
    public LeaveRecordPluginProvider(LeaveRecordPlugin plugin) : base(plugin)
    {
    }

    public override string Name => "LeaveRecord";
}
```

## Configuration

### Module Dependencies

To use the Semantic Kernel integration, add the following dependencies to your module:

```csharp
[DependsOn(
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

### 1. Plugin Implementation

Create a new plugin class that inherits from `ApplicationService` and implements `ITransientDependency`:

```csharp
public class YourPlugin : ApplicationService, ITransientDependency
{
    private readonly IYourAppService _yourService;
    private readonly IAuthorizationService _authorizationService;

    public YourPlugin(IYourAppService yourService, IAuthorizationService authorizationService) 
    {
        _yourService = yourService;
        _authorizationService = authorizationService;
    }

    [KernelFunction, Description("Your function description")]
    public async Task<string> YourFunctionAsync()
    {
        if (!await _authorizationService.IsGrantedAsync(YourPermissions.Default))
        {
            return "You are not authorized to access this data";
        }

        var result = await _yourService.GetListAsync();
        return JsonSerializer.Serialize(result);
    }
}
```

### 2. Provider Implementation

Create a provider class that inherits from `SemanticPluginProviderBase<TPlugin>`:

```csharp
public class YourPluginProvider : SemanticPluginProviderBase<YourPlugin>
{
    public YourPluginProvider(YourPlugin plugin) : base(plugin)
    {
    }

    public override string Name => "YourPlugin";
}
```

### 3. Example Queries and Responses

The Semantic Kernel integration can be accessed through the `/askai` endpoint. Here are some example interactions:

#### Example 1: Employee List Query

**Request:**
```http
POST /askai
Content-Type: application/json

{
    "question": "Give me the list of employees"
}
```

**Response:**
```json
{
    "answer": "Here is the list of employees:\n\n1. **John Doe**\n   - Email: john.doe@example.com\n   - Phone: 1234567890\n   - Date of Birth: January 1, 1990\n   - Joining Date: January 1, 2020\n   - Total Leave Days: 20\n   - Remaining Leave Days: 20\n\n2. **Jane Smith**\n   - Email: jane.smith@example.com\n   - Phone: 0987654321\n   - Date of Birth: February 1, 1991\n   - Joining Date: February 1, 2021\n   - Total Leave Days: 20\n   - Remaining Leave Days: 15"
}
```

#### Example 2: Leave Records Query

**Request:**
```http
POST /askai
Content-Type: application/json

{
    "question": "Give me the list of employee leave records"
}
```

**Response:**
```json
{
    "answer": "Here is the list of employee leave records:\n\n1. **John Doe**\n   - Annual leave from January 1 to January 5, 2025 (5 days)\n   - Sick leave from March 15 to March 16, 2025 (2 days)\n   - Personal leave from June 1 to June 3, 2025 (3 days)\n\n2. **Jane Smith**\n   - Sick leave from February 1 to February 3, 2025 (3 days)\n   - Annual leave from July 1 to July 10, 2025 (10 days)"
}
```

### 4. Authorization

All requests are protected by ABP's permission system. If a user doesn't have the required permissions, they will receive an appropriate message:

```json
{
    "answer": "You are not authorized to access this data"
}
```

The authorization check is performed in each plugin method:

```csharp
if (!await _authorizationService.IsGrantedAsync(YourPermissions.Default))
{
    return "You are not authorized to access this data";
}
```

### 5. UI Integration

> Note: An interactive chat UI for the AI assistant is coming soon.

## Security

The implementation includes built-in authorization checks using ABP's permission system. Each plugin method should check for appropriate permissions before executing sensitive operations.

## Contributing

Contributions are welcome! Please feel free to submit a Pull Request.

## License

This project is licensed under the MIT License - see the LICENSE file for details.
