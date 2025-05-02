using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using Volo.Abp.Timing;

namespace Wafi.SmartHR.Web.Pages.Chats;

public class IndexModel : PageModel
{
    [BindProperty]
    public string UserMessage { get; set; }

    [TempData]
    public string ConversationJson { get; set; }

    public List<Message> Conversation { get; set; } = new();

    public void OnGet()
    {
        LoadConversationFromTempData();
        
        if (Conversation == null || Conversation.Count == 0)
        {
            Conversation = new List<Message>
            {
                new Message
                {
                    Sender = "SmartHR",
                    Content = "Hello! I'm your SmartHR Assistant. How can I help you with employee and leave record queries today?",
                    CreationTime = DateTime.Now
                }
            };
            SaveConversationToTempData();
        }
    }

    private void LoadConversationFromTempData()
    {
        if (!string.IsNullOrEmpty(ConversationJson))
        {
            Conversation = JsonSerializer.Deserialize<List<Message>>(ConversationJson) ?? new();
        }
    }

    private void SaveConversationToTempData()
    {
        ConversationJson = JsonSerializer.Serialize(Conversation);
    }

    public class Message
    {
        public string Sender { get; set; }
        public string Content { get; set; }
        public DateTime CreationTime { get; set; }
    }

    public class AIResponse
    {
        public string Response { get; set; }
    }
}
