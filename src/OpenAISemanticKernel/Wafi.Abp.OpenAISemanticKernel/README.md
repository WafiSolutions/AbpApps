# Wafi.Abp.OpenAISemanticKernel Module

This module provides integration between ABP.io and Microsoft's Semantic Kernel, making it easy to add AI capabilities to your ABP applications.

## Features

- Seamless integration with OpenAI's models
- Auto API controllers for AI interactions
- Support for custom AI plugins
- Chat history management
- System message customization

## Installation

1. Add the Wafi.Abp.OpenAISemanticKernel package to your project:

```bash
dotnet add package Wafi.Abp.OpenAISemanticKernel
```

2. Add the module dependency to your ABP module:

```csharp
[DependsOn(
    // other dependencies
    typeof(WafiOpenAISemanticKernelModule)
)]
public class YourAppModule : AbpModule
{
    // ...
}
```

3. Configure OpenAI settings in your `appsettings.json`:

```json
{
  "SemanticKernel": {
    "OpenAI": {
      "ApiKey": "your-openai-key",
      "ModelId": "gpt-4" // or any other model you want to use
    }
  }
}
```

## Using the Auto API Controller

The module automatically exposes an API endpoint at `/api/ai/a-i` (or `/api/ai/ai` depending on your ABP configuration) that you can use to interact with the AI.

### Example Request

```http
POST /api/ai/a-i/ask
Content-Type: application/json

{
  "question": "What is Semantic Kernel?",
  "systemMessage": "You are a helpful assistant specialized in explaining technical concepts."
}
```

### Example Response

```json
{
  "answer": "Semantic Kernel is a framework developed by Microsoft that enables integration of AI capabilities into software applications. It provides a way to orchestrate AI models like GPT with conventional programming languages..."
}
```

## Advanced Configuration

If you need to customize the behavior of the API or chat completion service, you can replace the default implementations:

```csharp
// In your module's ConfigureServices method:
context.Services.Replace(ServiceDescriptor.Singleton<IWafiChatCompletionService, YourCustomService>());
context.Services.Replace(ServiceDescriptor.Transient<IAIAppService, YourCustomAIAppService>());
```

## Current User Information

The service automatically includes the current user's name in the system message if available. This helps personalize the AI responses based on who is interacting with it.

## Creating Custom Plugins

You can create custom AI plugins to extend functionality:

1. Create a plugin class:

```csharp
public class YourPlugin : ApplicationService, ITransientDependency
{
    [KernelFunction, Description("Your function description")]
    public async Task<string> YourFunctionAsync()
    {
        // Implementation
        return "Result";
    }
}
```

2. Create a provider:

```csharp
public class YourPluginProvider : SemanticPluginProviderBase<YourPlugin>
{
    public YourPluginProvider(YourPlugin plugin) : base(plugin) { }
    public override string Name => "YourPlugin";
}
```

For more details, see the full documentation. 