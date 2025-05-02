# Wafi.Abp.OpenAISemanticKernel Module

This module provides integration between ABP.io and Microsoft's Semantic Kernel, making it easy to add AI capabilities to your ABP applications.

## Features

- Seamless integration with OpenAI's models via Microsoft Semantic Kernel
- Direct chat completion services for AI interactions
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
      "ModelId": "gpt-4o-mini" // or any other model you want to use
    }
  }
}
```

## Using the Chat Completion Service

The primary way to interact with the AI is through the `IWafiChatCompletionService` interface:

```csharp
public class YourService
{
    private readonly IWafiChatCompletionService _chatService;
    
    public YourService(IWafiChatCompletionService chatService)
    {
        _chatService = chatService;
    }
    
    public async Task<string> GetAIResponseAsync(string question)
    {
        var history = new WafiChatHistory();
        
        // Optional: Add a system message to control AI behavior
        history.AddSystemMessage("You are a helpful assistant specialized in business applications.");
        
        // Ask the question and get the response
        return await _chatService.AskAsync(question, history);
    }
}
```

### Working with Chat History

You can maintain context across multiple interactions:

```csharp
// Create a history object once and reuse it for the conversation
var history = new WafiChatHistory();
history.AddSystemMessage("You are a helpful assistant.");

// First interaction
var firstResponse = await _chatService.AskAsync("What is Semantic Kernel?", history);

// Second interaction (history now contains the previous Q&A)
var secondResponse = await _chatService.AskAsync("How does it compare to LangChain?", history);
```

## Implementing a Custom Controller

To create your own controller for AI interactions:

```csharp
[Route("api/my-ai")]
public class MyAIController : AbpController
{
    private readonly IWafiChatCompletionService _chatService;
    private readonly ICurrentUser _currentUser;
    
    public MyAIController(IWafiChatCompletionService chatService, ICurrentUser currentUser)
    {
        _chatService = chatService;
        _currentUser = currentUser;
    }
    
    [HttpPost("ask")]
    public async Task<IActionResult> AskAsync([FromBody] MyQuestionModel input)
    {
        var history = new WafiChatHistory();
        
        history.AddSystemMessage(input.SystemMessage ?? "You are a helpful assistant.");
        
        // Add user context if needed
        if (_currentUser.Id.HasValue)
        {
            history.AddSystemMessage($"Current user: {_currentUser.Name}");
        }
        
        var answer = await _chatService.AskAsync(input.Question, history);
        
        return Ok(new { Answer = answer });
    }
}

public class MyQuestionModel
{
    [Required]
    public string Question { get; set; }
    
    public string SystemMessage { get; set; }
}
```

## Advanced Configuration

If you need to customize the behavior of the chat completion service, you can replace the default implementation:

```csharp
// In your module's ConfigureServices method:
context.Services.Replace(ServiceDescriptor.Singleton<IWafiChatCompletionService, YourCustomService>());
```

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

## Plugin Registration

Register your custom plugins in your module's `ConfigureServices` method:

```csharp
public override void ConfigureServices(ServiceConfigurationContext context)
{
    // Register plugin provider
    context.Services.AddTransient<IWafiPluginProvider, YourPluginProvider>();
}
```

## Testing

When testing, configure your test module to use the Semantic Kernel services:

```csharp
public class YourTestModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        var configuration = context.Services.GetConfiguration();
        
        // Configure OpenAI settings for testing
        Configure<WafiOpenAISemanticKernelOptions>(options =>
        {
            options.ModelId = configuration.GetValue<string>("SemanticKernel:OpenAI:ModelId");
            options.ApiKey = configuration.GetValue<string>("SemanticKernel:OpenAI:ApiKey");
        });
    }
}
```

For more details, see the full documentation. 