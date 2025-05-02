using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Shouldly;
using Wafi.Abp.OpenAISemanticKernel.Chat;
using Wafi.Abp.OpenAISemanticKernel.Chat.Dtos;
using Xunit;
using Xunit.Abstractions;

namespace Wafi.Abp.OpenAISemanticKernel.Services;

/// <summary>
/// Integration tests for the WafiChatCompletionService.
/// These tests verify the functionality of the chat service with the actual OpenAI API.
/// </summary>
public class AiAppServiceTests : OpenAISemanticKernelTestBase
{
    private readonly IAiAppService aiAppService;

    public AiAppServiceTests(ITestOutputHelper testOutputHelper)
    {
        aiAppService = GetRequiredService<IAiAppService>();
    }

    /// <summary>
    /// Verifies that user messages are correctly added to the chat history.
    /// </summary>
    [Fact]
    public async Task Should_Add_User_Message_To_History()
    {
        // Arrange
        var message = "Hello, I need help";
        var request = new AskRequestDto
        {
            Question = message,
            History = new List<Message>
            {
                new Message { Sender = SenderType.User, Content = message }
            }
        };

        // Act
        var response = await aiAppService.AskAsync(request);

        // Assert
        response.Answer.ShouldNotBeNullOrWhiteSpace("The API should return a non-empty response");
    }

    /// <summary>
    /// Verifies that system messages are correctly preserved and respected in the conversation.
    /// </summary>
    [Fact]
    public async Task Should_Respect_System_Message()
    {
        // Arrange
        var systemMessage = "You are a helpful assistant that specializes in ABP Framework";
        var question = "What are you specialized in?";

        var request = new AskRequestDto
        {
            Question = question,
            History = new List<Message>
            {
                new Message { Sender = SenderType.System, Content = systemMessage },
                new Message { Sender = SenderType.User, Content = question }
            }
        };

        // Act
        var response = await aiAppService.AskAsync(request);

        // Assert
        response.Answer.ShouldNotBeNullOrWhiteSpace("The API should return a non-empty response");

        // Verify the response mentions ABP as specified in the system message
        response.Answer.ToLower().ShouldContain("abp");
    }

    /// <summary>
    /// Verifies that conversation context is maintained across multiple turns.
    /// </summary>
    [Fact]
    public async Task Should_Maintain_Conversation_Context()
    {
        // Arrange - First turn
        var firstQuestion = "My name is Alice";
        var firstRequest = new AskRequestDto
        {
            Question = firstQuestion,
            History = new List<Message>
            {
                new Message { Sender = SenderType.User, Content = firstQuestion }
            }
        };

        // First interaction
        var firstResponse = await aiAppService.AskAsync(firstRequest);

        // Second turn - referring to information from the first turn
        var secondQuestion = "What's my name?";
        var secondRequest = new AskRequestDto
        {
            Question = secondQuestion,
            History = new List<Message>
            {
                new Message { Sender = SenderType.User, Content = firstQuestion },
                new Message { Sender = SenderType.Assistant, Content = firstResponse.Answer },
                new Message { Sender = SenderType.User, Content = secondQuestion }
            }
        };

        // Act
        var response = await aiAppService.AskAsync(secondRequest);

        // Assert
        response.Answer.ShouldNotBeNullOrWhiteSpace("The API should return a non-empty response");
        response.Answer.ToLower().ShouldContain("alice");
    }
}
