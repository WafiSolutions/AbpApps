using Shouldly;
using Xunit;

namespace Wafi.Abp.OpenAISemanticKernel.Services;

public class WafiChatCompletionServiceTests : OpenAISemanticKernelTestBase
{
    private readonly IWafiChatCompletionService _chatCompletionService;

    public WafiChatCompletionServiceTests()
    {
        _chatCompletionService = GetRequiredService<IWafiChatCompletionService>();
    }

    [Fact]
    public void Should_Resolve_WafiChatCompletionService()
    {
        // Arrange & Act & Assert
        _chatCompletionService.ShouldNotBeNull();
        _chatCompletionService.ShouldBeOfType<WafiChatCompletionService>();
    }
}
