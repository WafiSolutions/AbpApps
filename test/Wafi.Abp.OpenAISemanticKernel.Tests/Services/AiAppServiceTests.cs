using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Shouldly;
using Wafi.Abp.OpenAISemanticKernel.Chat;
using Wafi.Abp.OpenAISemanticKernel.Chat.Dtos;
using Wafi.Abp.OpenAISemanticKernel.Chat.Enums;
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
    /// Verifies that ai is responding.
    /// </summary>
    [Fact]
    public async Task Should_Return_Answer_When_Asked()
    {
        // Arrange
        var message = "Hello, I need help";
        var request = new AskRequestDto
        {
            Question = message,
            History = [
                new() { UserType = UserType.User, Content = message }
            ]
        };

        // Act
        var response = await aiAppService.AskAsync(request);

        // Assert
        response.Answer.ShouldNotBeNullOrWhiteSpace("The API should return a non-empty response");
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
            History = [
                new() { UserType = UserType.User, Content = firstQuestion }
            ]
        };

        // First interaction
        var firstResponse = await aiAppService.AskAsync(firstRequest);

        // Second turn - referring to information from the first turn
        var secondQuestion = "What's my name?";
        var secondRequest = new AskRequestDto
        {
            Question = secondQuestion,
            History = [
                new() { UserType = UserType.User, Content = firstQuestion },
                new() { UserType = UserType.Ai, Content = firstResponse.Answer },
                new() { UserType = UserType.User, Content = secondQuestion }
            ]
        };

        // Act
        var response = await aiAppService.AskAsync(secondRequest);

        // Assert
        response.Answer.ShouldNotBeNullOrWhiteSpace("The API should return a non-empty response");
        response.Answer.ToLower().ShouldContain("alice");
    }
}
