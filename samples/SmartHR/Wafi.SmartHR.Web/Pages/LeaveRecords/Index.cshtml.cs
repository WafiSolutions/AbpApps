using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Wafi.SmartHR.Permissions;

namespace Wafi.SmartHR.Web.Pages.LeaveRecords;


[Authorize(SmartHRPermissions.LeaveRecords.Default)]
public class IndexModel : SmartHRPageModel
{
    public async Task OnGetAsync()
    {
        await Task.CompletedTask;
    }
}
