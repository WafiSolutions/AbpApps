using System.Linq;
using System.Threading.Tasks;
using Shouldly;
using Xunit;
using Xunit.Abstractions;

namespace Wafi.Abp.OpenAISemanticKernel.Services;

/// <summary>
/// Integration tests for the WafiChatCompletionService.
/// These tests verify the functionality of the chat service with the actual OpenAI API.
/// </summary>
public class WafiChatTests : OpenAISemanticKernelTestBase
{
    private readonly IWafiChatCompletionService _chatCompletionService;

    public WafiChatTests(ITestOutputHelper testOutputHelper)
    {
        _chatCompletionService = GetRequiredService<IWafiChatCompletionService>();
    }

    /// <summary>
    /// Verifies that user messages are correctly added to the chat history.
    /// </summary>
    [Fact]
    public async Task Should_Add_User_Message_To_History()
    {
        // Arrange
        var history = new WafiChatHistory();
        var message = "Hello, I need help";
        history.AddUserMessage(message);

        // Act
        var response = await _chatCompletionService.AskAsync(message, history);

        // Assert
        response.ShouldNotBeNullOrWhiteSpace("The API should return a non-empty response");

        var messages = history.Messages.ToList();
        messages.Count.ShouldBe(2, "The history should contain both user and assistant messages");
        messages[0].Role.ShouldBe("user", "First message should be from the user");
        messages[0].Message.ShouldBe(message, "The user message content should match the input");
        messages[1].Role.ShouldBe("assistant", "Second message should be from the assistant");
        messages[1].Message.ShouldBe(response, "The assistant message should match the API response");
    }

    /// <summary>
    /// Verifies that system messages are correctly preserved and respected in the conversation.
    /// </summary>
    [Fact]
    public async Task Should_Respect_System_Message()
    {
        // Arrange
        var history = new WafiChatHistory();
        var systemMessage = "You are a helpful assistant that specializes in ABP Framework";
        history.AddSystemMessage(systemMessage);

        var question = "What are you specialized in?";
        history.AddUserMessage(question);

        // Act
        var response = await _chatCompletionService.AskAsync(question, history);

        // Assert
        response.ShouldNotBeNullOrWhiteSpace("The API should return a non-empty response");

        var messages = history.Messages.ToList();
        messages.Count.ShouldBe(3, "The history should contain system, user, and assistant messages");
        messages[0].Role.ShouldBe("system", "First message should be the system prompt");
        messages[0].Message.ShouldBe(systemMessage, "The system message should be preserved");

        // Verify the response mentions ABP as specified in the system message
        response.ToLower().ShouldContain("abp");
    }

    /// <summary>
    /// Verifies that conversation context is maintained across multiple turns.
    /// </summary>
    [Fact]
    public async Task Should_Maintain_Conversation_Context()
    {
        // Arrange
        var history = new WafiChatHistory();

        // First turn
        var firstQuestion = "My name is Alice";
        history.AddUserMessage(firstQuestion);
        await _chatCompletionService.AskAsync(firstQuestion, history);

        // Second turn - referring to information from the first turn
        var secondQuestion = "What's my name?";
        history.AddUserMessage(secondQuestion);

        // Act
        var response = await _chatCompletionService.AskAsync(secondQuestion, history);

        // Assert
        response.ShouldNotBeNullOrWhiteSpace("The API should return a non-empty response");
        response.ToLower().ShouldContain("alice");
    }
}
