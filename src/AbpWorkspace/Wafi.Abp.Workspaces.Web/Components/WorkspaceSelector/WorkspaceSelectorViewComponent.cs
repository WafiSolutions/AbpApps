using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc;
using Wafi.Abp.Workspaces.Localization;

namespace Wafi.Abp.Workspaces.Web.Components.WorkspaceSelector;

public class WorkspaceSelectorViewComponent : AbpViewComponent
{
    public virtual IViewComponentResult Invoke()
    {
        return View("~/Components/WorkspaceSelector/Default.cshtml");
    }
} 
