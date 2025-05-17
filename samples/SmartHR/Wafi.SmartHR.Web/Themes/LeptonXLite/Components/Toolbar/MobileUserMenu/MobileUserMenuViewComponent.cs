using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Volo.Abp.UI.Navigation;

namespace Volo.Abp.AspNetCore.Mvc.UI.Theme.LeptonXLite.Themes.LeptonXLite.Components.Toolbar.MobileUserMenu;

public class MobileUserMenuViewComponent : AbpViewComponent
{
    protected IMenuManager MenuManager { get; }

    public MobileUserMenuViewComponent(IMenuManager menuManager)
    {
        MenuManager = menuManager;
    }

    public virtual async Task<IViewComponentResult> InvokeAsync()
    {
        var menu = await MenuManager.GetAsync(StandardMenus.User);
        return View("~/Themes/LeptonXLite/Components/Toolbar/MobileUserMenu/Default.cshtml", menu);
    }
}
