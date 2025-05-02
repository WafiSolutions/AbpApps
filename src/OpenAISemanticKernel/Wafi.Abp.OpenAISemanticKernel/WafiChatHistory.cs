using System.Collections.Generic;
using Wafi.Abp.OpenAISemanticKernel.Chat.Dtos;

namespace Wafi.Abp.OpenAISemanticKernel;

public class WafiChatHistory
{
    private readonly List<(string Role, string Message)> _messages = new();

    public void AddUserMessage(string message)
    {
        _messages.Add(("user", message));
    }

    public void AddUserMessages(List<Message> messages)
    {
        foreach (var message in messages)
        {
            var role = message.Sender switch
            {
                SenderType.user => "user",
                SenderType.ai => "assistant",
                _ => "user" // default to user if unknown
            };
            
            _messages.Add((role, message.Content));
        }
    }

    public void AddAssistantMessage(string message)
    {
        _messages.Add(("assistant", message));
    }

    public void AddSystemMessage(string message)
    {
        _messages.Add(("system", message));
    }

    public IEnumerable<(string Role, string Message)> Messages => _messages;
}
