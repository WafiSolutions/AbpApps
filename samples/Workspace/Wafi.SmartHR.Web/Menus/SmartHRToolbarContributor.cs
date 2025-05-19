using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.LeptonXLite;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.LeptonXLite.Toolbars;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.Shared.Toolbars;
using Volo.Abp.Users;
using Wafi.Abp.Workspaces.Web.Components.WorkspaceSelector;

namespace Wafi.SmartHR.Web.Menus;

public class SmartHRToolbarContributor : IToolbarContributor
{
    public async Task ConfigureToolbarAsync(IToolbarConfigurationContext context)
    {
        if (!(context.Theme is LeptonXLiteTheme))
        {
            return;
        }

        if (context.Toolbar.Name == LeptonXLiteToolbars.Main)
        {
            if (context.ServiceProvider.GetRequiredService<ICurrentUser>().IsAuthenticated)
            {
                context.Toolbar.Items.AddFirst(new ToolbarItem(typeof(WorkspaceSelectorViewComponent)));
            }
        }
    }
}
