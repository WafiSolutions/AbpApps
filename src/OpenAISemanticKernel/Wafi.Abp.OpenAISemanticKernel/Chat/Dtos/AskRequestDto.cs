using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Wafi.Abp.OpenAISemanticKernel.Chat.Dtos;

/// <summary>
/// Represents a request to ask a question to the AI assistant
/// </summary>
public class AskRequestDto
{
    /// <summary>
    /// The question to ask the AI assistant
    /// </summary>
    [Required]
    [StringLength(4000, MinimumLength = 1)]
    public string Question { get; set; }
    
    /// <summary>
    /// Conversation history
    /// </summary>
    public List<string> History { get; set; }
} 
