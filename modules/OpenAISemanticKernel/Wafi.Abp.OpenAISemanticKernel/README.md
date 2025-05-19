## Wafi.Abp.OpenAISemanticKernel Module

The module is built around these key components:

### Main Module Class

`WafiOpenAISemanticKernelModule` configures the services and integrates with ABP's dependency injection system. It registers:

- The Semantic Kernel instance
- The chat completion service
- Automatic API controller registration
- Plugin providers

### Chat Components

- `IWafiChatCompletionService`: Core interface for interacting with AI models
- `WafiChatCompletionService`: Implementation that converts ABP abstractions to Semantic Kernel calls
- `WafiChatHistory`: Manages conversation history with different sender types (User, Assistant, System)
- `AiAppService`: Auto-exposed API controller for chat interactions

### Plugin System

- `IWafiPluginProvider`: Interface for custom plugin providers
- `SemanticPluginProviderBase<T>`: Base class to simplify creating custom plugin providers
- `WafiKernelPlugin`: Model that represents a plugin to be registered with Semantic Kernel

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
        
        // Add a system message to control AI behavior
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

## Using the Built-in API Controller

The module auto-registers an API controller at `/api/openAISemanticKernel/ai` with:

- `AskAsync` endpoint that accepts an `AskRequestDto` with:
  - Question (required)
  - Optional conversation history

Example API call:

```http
POST /api/openAISemanticKernel/ai/ask
{
  "question": "What is ABP.io?",
  "history": [] // Optional previous messages
}
```

## Creating Custom Plugins

1. Create a plugin class with methods decorated with `KernelFunction` attribute:

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

2. Create a provider by inheriting from the base class:

```csharp
public class YourPluginProvider : SemanticPluginProviderBase<YourPlugin>
{
    public YourPluginProvider(YourPlugin plugin) : base(plugin) { }
    public override string Name => "YourPlugin";
}
```

3. Register your provider in your module's `ConfigureServices` method:

```csharp
public override void ConfigureServices(ServiceConfigurationContext context)
{
    // Register plugin provider
    context.Services.AddTransient<IWafiPluginProvider, YourPluginProvider>();
}
```

## Advanced Configuration

If you need to customize the behavior of the chat completion service, you can replace the default implementation:

```csharp
// In your module's ConfigureServices method:
context.Services.Replace(ServiceDescriptor.Singleton<IWafiChatCompletionService, YourCustomService>());
```

## Dependencies

The module depends on:
- Microsoft.SemanticKernel.Connectors.OpenAI (1.47.0)
- Volo.Abp.Core (9.1.0)
- Volo.Abp.AspNetCore.Mvc (9.1.0) 