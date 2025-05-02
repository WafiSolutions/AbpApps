using System.Threading.Tasks;
using DeviceDetectorNET;
using Volo.Abp.Application.Services;
using Wafi.Abp.OpenAISemanticKernel.Services.Chat;
using Wafi.Abp.OpenAISemanticKernel.Services.Dtos;

namespace Wafi.Abp.OpenAISemanticKernel.Services;

/// <summary>
/// Application service implementation for AI chat completions
/// </summary>
public class AiAppService(IWafiChatCompletionService aiAppService)
    : ApplicationService, IAiAppService
{

    /// <summary>
    /// Asks a question to the AI assistant and returns the response
    /// </summary>
    /// <param name="input">The request containing the question and optional system message</param>
    /// <returns>The AI assistant's response</returns>
    public async Task<AskResponseDto> AskAsync(AskRequestDto input)
    {
        var history = new WafiChatHistory();

        history.AddSystemMessage(@"This is sample HR applicaiton for testing Microsoft Semantic Kernel Chat Completion functionality");

        history.AddSystemMessage($"Current logged user name {CurrentUser.Name + " " + CurrentUser.SurName}");


        if (input.History.Any())
        {
            history.AddUserMessages(input.History);
        }


        var answer = await aiAppService.AskAsync(input.Question, history);

        return new AskResponseDto
        {
            Answer = answer
        };
    }
}
