using System.Threading.Tasks;

namespace Wafi.Abp.OpenAISemanticKernel.Services;

public interface IWafiChatCompletionService
{
    Task<string> AskAsync(string question, WafiChatHistory history);
}
