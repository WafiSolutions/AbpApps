using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wafi.Abp.OpenAISemanticKernel
{
    public class WafiChatHistory
    {
        private readonly List<(string Role, string Message)> _messages = new();

        public void AddUserMessage(string message)
        {
            _messages.Add(("user", message));
        }

        public void AddAssistantMessage(string message)
        {
            _messages.Add(("assistant", message));
        }

        public void AddSystemMessage(string message)
        {
            _messages.Add(("system", message));
        }

        public IEnumerable<(string Role, string Message)> Messages => _messages;
    }
}
