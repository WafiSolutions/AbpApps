using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Wafi.Abp.OpenAISemanticKernel.Services.Dtos;

namespace Wafi.Abp.OpenAISemanticKernel.Services;

/// <summary>
/// Application service interface for AI chat completions
/// </summary>
public interface IAiAppService : IApplicationService
{
    /// <summary>
    /// Asks a question to the AI assistant
    /// </summary>
    /// <param name="input">The request containing the question and optional system message</param>
    /// <returns>The AI assistant's response</returns>
    Task<AskResponseDto> AskAsync(AskRequestDto input);
}
