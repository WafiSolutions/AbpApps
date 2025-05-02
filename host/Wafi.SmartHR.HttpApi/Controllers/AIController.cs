using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.AspNetCore.Mvc;
using Wafi.Abp.OpenAISemanticKernel;
using Wafi.SmartHR.Models.AI;

namespace Wafi.SmartHR.Controllers
{
    [Authorize]
    public class SmartAIController : AbpControllerBase
    {
        private readonly IWafiChatCompletionService _aiService;

        public SmartAIController(IWafiChatCompletionService aiService)
        {
            _aiService = aiService;
        }

        [HttpPost("askai")]
        public async Task<IActionResult> AskAsync([FromBody] AskRequest input)
        {
            var history = new WafiChatHistory();

            history.AddSystemMessage(@"This is sample HR applicaiton for testing Microsoft Semantic Kernel Chat Completion functionality");

            history.AddSystemMessage($"Current logged user name {CurrentUser.Name + " " + CurrentUser.SurName}");


            var answer = await _aiService.AskAsync(input.Question, history);
            return Ok(new { Answer = answer });
        }
    }
}
