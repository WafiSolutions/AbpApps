using System;

namespace Wafi.Abp.OpenAISemanticKernel.Chat.Dtos;

public class Message
{
    public SenderType Sender { get; set; }
    public string Content { get; set; }
    public DateTime CreationTime { get; set; }
}