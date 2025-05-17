using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.LeptonXLite.Toolbars;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.Shared.Toolbars;

namespace Volo.Abp.AspNetCore.Mvc.UI.Theme.LeptonXLite.Themes.LeptonXLite.Components.MainToolbar;

public class ToolbarItemsViewComponent : AbpViewComponent
{
    private IToolbarManager _toolbarManager;

    public ToolbarItemsViewComponent(IToolbarManager toolbarManager)
    {
        _toolbarManager = toolbarManager;
    }

    public async Task<IViewComponentResult> InvokeAsync(string name)
    {
        var toolbar = await _toolbarManager.GetAsync(name ?? LeptonXLiteToolbars.Main);
        return View("~/Themes/LeptonXLite/Components/ToolbarItems/Default.cshtml", toolbar);
    }
}
