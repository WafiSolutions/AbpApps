using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wafi.Abp.OpenAISemanticKernel
{
    public interface IWafiChatCompletionService
    {
        Task<string> AskAsync(string question, WafiChatHistory history);
    }
}
