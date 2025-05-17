using System;
using System.Collections.Generic;
using System.Text.Json;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Wafi.SmartHR.Permissions;

namespace Wafi.SmartHR.Web.Pages.Chats;

[Authorize(SmartHRPermissions.LeaveRecords.Default)]
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
}

