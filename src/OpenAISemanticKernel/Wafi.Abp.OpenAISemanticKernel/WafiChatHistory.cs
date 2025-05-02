using System.Collections.Generic;
using Wafi.Abp.OpenAISemanticKernel.Chat.Dtos;

namespace Wafi.Abp.OpenAISemanticKernel;

public class WafiChatHistory
{
    public readonly List<(SenderType Role, string Message)> Messages = new();

    public void AddUserMessage(string message)
    {
        Messages.Add((SenderType.User, message));
    }

    public void AddConversationHistory(List<Message> messages)
    {
        foreach (var message in messages)
        {
            var role = ConvertToSenderType(message.UserType);
            Messages.Add((role, message.Content));

        }
    }

    public void AddAssistantMessage(string message)
    {
        Messages.Add((SenderType.Assistant, message));
    }

    public void AddSystemMessage(string message)
    {
        Messages.Add((SenderType.System, message));
    }

    private SenderType ConvertToSenderType(UserType userType)
    {
        return userType switch
        {
            UserType.User => SenderType.User,
            UserType.Ai => SenderType.Assistant,
            _ => SenderType.User
        };
    }

}
