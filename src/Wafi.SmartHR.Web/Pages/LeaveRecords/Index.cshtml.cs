using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;
using Wafi.SmartHR.LeaveRecords;

namespace Wafi.SmartHR.Web.Pages.LeaveRecords
{
    public class IndexModel : SmartHRPageModel
    {
        private readonly ILeaveRecordAppService _leaveRecordAppService;

        public IndexModel(ILeaveRecordAppService leaveRecordAppService)
        {
            _leaveRecordAppService = leaveRecordAppService;
        }

        public async Task OnGetAsync()
        {
            await Task.CompletedTask;
        }
    }
} 