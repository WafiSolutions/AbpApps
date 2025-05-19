using System.Threading.Tasks;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.OpenAI;
using Wafi.Abp.OpenAISemanticKernel.Chat.Enums;

namespace Wafi.Abp.OpenAISemanticKernel.Services;

public class WafiChatCompletionService : IWafiChatCompletionService
{
    private readonly Kernel _kernel;
    private readonly IChatCompletionService _chat;

    public WafiChatCompletionService(Kernel kernel)
    {
        _kernel = kernel;
        _chat = _kernel.GetRequiredService<IChatCompletionService>();
    }

    public async Task<string> AskAsync(string question, WafiChatHistory wafiHistory)
    {
        var history = new ChatHistory();

        // Convert WafiChatHistory to ChatHistory
        foreach (var (role, message) in wafiHistory.Messages)
        {
            if (role == SenderType.User)
                history.AddUserMessage(message);
            else if (role == SenderType.Assistant)
                history.AddAssistantMessage(message);
            else if (role == SenderType.System)
                history.AddSystemMessage(message);
        }

        history.AddUserMessage(question);

        var executionSettings = new OpenAIPromptExecutionSettings
        {
            ToolCallBehavior = ToolCallBehavior.AutoInvokeKernelFunctions,
            MaxTokens = 100
        };

        var result = await _chat.GetChatMessageContentsAsync(history, executionSettings, _kernel);
        var answer = result[^1].Content;

        history.AddAssistantMessage(answer);
        wafiHistory.AddAssistantMessage(answer); // Keep user-side history updated

        return answer;
    }
}
