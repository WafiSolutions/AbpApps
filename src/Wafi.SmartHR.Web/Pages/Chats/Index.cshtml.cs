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

public class IndexModel(HttpClient httpClient) : PageModel
{
    [BindProperty]
    public string UserMessage { get; set; }

    [TempData]
    public string ConversationJson { get; set; }

    public List<Message> Conversation { get; set; } = new();

    public void OnGet()
    {
        LoadConversationFromTempData();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        LoadConversationFromTempData();

        if (!string.IsNullOrWhiteSpace(UserMessage))
        {
            // Add user message
            Conversation.Add(new Message { Sender = "User", Content = UserMessage });

            // Send to AI API
            //var response = await httpClient.PostAsJsonAsync("/askai", new { input = UserMessage });
            if (true)
            {
                //var aiReply = await response.Content.ReadFromJsonAsync<AIResponse>();
                Conversation.Add(new Message { Sender = "AI", Content = "(no reply)" });
            }
            else
            {
                Conversation.Add(new Message { Sender = "AI", Content = "Error getting response from AI.", CreationTime = DateTime.Now });
            }
        }

        SaveConversationToTempData();

        ModelState.Clear();
        return Page();
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
