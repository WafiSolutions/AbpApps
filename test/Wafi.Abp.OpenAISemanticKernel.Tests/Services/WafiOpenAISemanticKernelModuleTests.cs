using Microsoft.Extensions.Options;
using Shouldly;
using Wafi.Abp.OpenAISemanticKernel.Services.Chat;
using Xunit;

namespace Wafi.Abp.OpenAISemanticKernel.Services;

public class WafiOpenAISemanticKernelModuleTests : OpenAISemanticKernelTestBase
{
    [Fact]
    public void Should_Register_WafiChatCompletionService()
    {
        // Act
        var service = GetService<IWafiChatCompletionService>();

        // Assert
        service.ShouldNotBeNull();
        service.ShouldBeOfType<WafiChatCompletionService>();
    }

    [Fact]
    public void Should_Configure_Options()
    {
        // Act
        var options = GetService<IOptions<WafiOpenAISemanticKernelOptions>>().Value;

        // Assert
        options.ShouldNotBeNull();
        options.ModelId.ShouldBe("test-model");
        options.ApiKey.ShouldBe("test-api-key");
    }
}
