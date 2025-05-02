using System.Linq;
using Shouldly;
using Xunit;

namespace Wafi.Abp.OpenAISemanticKernel.Services;

public class WafiChatHistoryTests
{
    [Fact]
    public void Should_Add_User_Message()
    {
        // Arrange
        var history = new WafiChatHistory();
        var message = "Hello, I need help";

        // Act
        history.AddUserMessage(message);

        // Assert
        var messages = history.Messages.ToList();
        messages.Count.ShouldBe(1);
        messages[0].Role.ShouldBe("user");
        messages[0].Message.ShouldBe(message);
    }

    [Fact]
    public void Should_Add_Assistant_Message()
    {
        // Arrange
        var history = new WafiChatHistory();
        var message = "I can help you with that";

        // Act
        history.AddAssistantMessage(message);

        // Assert
        var messages = history.Messages.ToList();
        messages.Count.ShouldBe(1);
        messages[0].Role.ShouldBe("assistant");
        messages[0].Message.ShouldBe(message);
    }

    [Fact]
    public void Should_Add_System_Message()
    {
        // Arrange
        var history = new WafiChatHistory();
        var message = "You are a helpful assistant";

        // Act
        history.AddSystemMessage(message);

        // Assert
        var messages = history.Messages.ToList();
        messages.Count.ShouldBe(1);
        messages[0].Role.ShouldBe("system");
        messages[0].Message.ShouldBe(message);
    }

    [Fact]
    public void Should_Maintain_Message_Order()
    {
        // Arrange
        var history = new WafiChatHistory();

        // Act
        history.AddSystemMessage("System instruction");
        history.AddUserMessage("User question");
        history.AddAssistantMessage("Assistant response");
        history.AddUserMessage("Follow-up question");

        // Assert
        var messages = history.Messages.ToList();
        messages.Count.ShouldBe(4);
        messages[0].Role.ShouldBe("system");
        messages[1].Role.ShouldBe("user");
        messages[2].Role.ShouldBe("assistant");
        messages[3].Role.ShouldBe("user");
    }
}
