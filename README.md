<h1 style="display: flex; align-items: center;">
  <img src="https://img.icons8.com/color/48/microsoft.png" alt="Microsoft Logo" style="vertical-align: middle; margin-right: 10px;">
  Using Semantic Kernel in an ABP Framework
</h1>

This guide demonstrates how to integrate **Microsoft's Semantic Kernel SDK** with the **ABP.io** framework. It includes a wrapper implementation to help you create AI-powered plugins for your ABP application services in a clean, modular way.


## 🏗️ Architecture Overview

The solution consists of two primary modules:

1. **`WafiOpenAISemanticKernelModule`**
   Core module that provides integration with Semantic Kernel.

2. **`WafiSmartHRAIPluginModule`**
   Sample implementation module demonstrating how to create AI plugins for business entities.


## ⚙️ Implementation Steps

Follow these steps to integrate Semantic Kernel into your ABP application.

---

### 🔹 Step 1: Add the Package to Your HttpApi Project

1. Add the Semantic Kernel wrapper package:

```bash
dotnet add YourApp.HttpApi.csproj package Wafi.Abp.OpenAISemanticKernel
```

2. Add the module dependency:

```csharp
[DependsOn(
    // your existing dependencies
    typeof(WafiOpenAISemanticKernelModule)
)]
public class YourAppHttpApiModule : AbpModule
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

3. Add the following configuration to your appsettings.json:

```
{
  "SemanticKernel": {
    "OpenAI": {
      "ApiKey": "your-openai-key",
      "ModelId": "gpt-4"
    }
  }
}
```


> ✅ This exposes the `/api/OpenAISemanticKernel/ai/ask` endpoint automatically, which the chat interface will use to interact with Semantic Kernel.

---

### 🔹 Step 2: Implement a Chat Interface

In your Web project, create a chat interface to interact with the `/ai/ask` API. You can refer to the host/`Wafi.SmartHR.Web` project for a sample implementation.

![chat-page-location](/etc/img/folder-chat-page.gif)

---

### 🔹 Step 3: Build Plugins to Connect ABP Services with Semantic Kernel

#### 1. Add dependencies to the plugin project:

```bash
dotnet add package Wafi.Abp.OpenAISemanticKernel
dotnet add reference ../YourApp.Application/YourApp.Application.csproj
```

Add this to your plugin module class:

```csharp
[DependsOn(typeof(YourAppApplicationModule))]
public class YourAppAIPluginModule : AbpModule
{
    // ...
}
```


#### 2. Create AI Plugins for Your Entities

Each plugin class should clearly describe what the AI will get when calling its methods.

📄 Example: `YourEntityPlugin.cs`

```csharp
public class YourEntityPlugin : ApplicationService, ITransientDependency
{
    private readonly IYourEntityAppService _entityService;
    private readonly IAuthorizationService _authorizationService;

    public YourEntityPlugin(
        IYourEntityAppService entityService,
        IAuthorizationService authorizationService)
    {
        _entityService = entityService;
        _authorizationService = authorizationService;
    }

    [KernelFunction]
    [Description("Search for entities by name or any relevant property using a keyword or phrase")]
    public async Task<string> SearchEntitiesAsync(string filter)
    {
        if (!await _authorizationService.IsGrantedAsync(YourPermissions.Default))
        {
            return "You are not authorized to access this data";
        }

        var entities = await _entityService.GetListAsync(filter);
        
        if (entities is null || entities.Items.Count == 0)
        {
            return $"No entities found for filter '{filter}'";
        }

        return JsonSerializer.Serialize(entities);
    }
}
```
> 🧠 Be sure to give a meaningful `[Description]` so that the AI knows exactly what this method does.


####  3. Create a Plugin Provider

```csharp
using Wafi.Abp.OpenAISemanticKernel.Plugins;
using YourApp.AI.Plugin.YourEntities;

namespace YourApp.AI.Plugin
{
    public class YourEntityPluginProvider : SemanticPluginProviderBase<YourEntityPlugin>, IWafiPluginProvider
    {
        public YourEntityPluginProvider(YourEntityPlugin plugin) : base(plugin) { }

        public override string Name => "Your Plugin Name";
    }
}
```


#### 🔐 Authorization

All requests are protected by ABP's permission system. If a user doesn't have the required permissions, they will receive an appropriate message:

> ⚠️ **You are not authorized to access this data.**

The authorization check is performed in each plugin method:

```csharp
if (!await _authorizationService.IsGrantedAsync(YourPermissions.Default))
{
    return "You are not authorized to access this data";
}
```


---


## ✅ Final Outcome 
The Semantic Kernel integration can be accessed through the `/askai` endpoint. Here are some example interactions:

![Demo](/etc/img/chat_sample.gif)

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
> Here is the list of employees: **John Doe**   - Email: john.doe@example.com   - Phone: 1234567890   - Date of Birth: January 1, 1990   - Joining Date: January 1, 2020   - Total Leave Days: 20   - Remaining Leave Days: 20. **Jane Smith**   - Email: jane.smith@example.com   - Phone: 0987654321   - Date of Birth: February 1, 1991   - Joining Date: February 1, 2021   - Total Leave Days: 20   - Remaining Leave Days: 15

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

>  Here is the list of employee leave records: **John Doe**   - Annual leave from January 1 to January 5, 2025 (5 days)  - Sick leave from March 15 to March 16, 2025 (2 days)\n   - Personal leave from June 1 to June 3, 2025 (3 days) **Jane Smith**   - Sick leave from February 1 to February 3, 2025 (3 days)   - Annual leave from July 1 to July 10, 2025 (10 days)"

---


## 📚 Additional Resources

For a deeper understanding of the module's implementation, architecture decisions, and advanced customization options, see:

- [Microsoft Semantic Kernel Documentation](https://learn.microsoft.com/en-us/semantic-kernel/overview/)
- [ABP Framework Documentation](https://docs.abp.io/en/abp/latest/)
- [Full Module Source Code](https://github.com/WafiSolutions/Wafi.Abp.SemanticKernel/tree/main/src/OpenAISemanticKernel/Wafi.Abp.OpenAISemanticKernel) 
