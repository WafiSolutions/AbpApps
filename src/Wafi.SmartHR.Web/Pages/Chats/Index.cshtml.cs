using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Wafi.SmartHR.Web.Pages.Chats;

public class IndexModel(HttpClient httpClient) : PageModel
{
    [BindProperty]
    public string UserMessage { get; set; }

    public List<Message> Conversation { get; set; } = new();

    public async Task<IActionResult> OnPostAsync()
    {
        if (!string.IsNullOrWhiteSpace(UserMessage))
        {
            // Add user's message
            Conversation.Add(new Message { Sender = "User", Content = UserMessage });

            // Send to AI API
            var response = await httpClient.PostAsJsonAsync("/askai", new { input = UserMessage });
            if (response.IsSuccessStatusCode)
            {
                var aiReply = await response.Content.ReadFromJsonAsync<AIResponse>();
                Conversation.Add(new Message { Sender = "AI", Content = aiReply?.Response });
            }
            else
            {
                Conversation.Add(new Message { Sender = "AI", Content = "Error getting response." });
            }
        }

        ModelState.Clear();
        return Page();
    }

    public class Message
    {
        public string Sender { get; set; }
        public string Content { get; set; }
    }

    public class AIResponse
    {
        public string Response { get; set; }
    }
}
