using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.LeptonXLite.Languages;

namespace Volo.Abp.AspNetCore.Mvc.UI.Theme.LeptonXLite.Themes.LeptonXLite.Components.Toolbar.MobileLanguageSwitch;

public class MobileLanguageSwitchViewComponent : AbpViewComponent
{
    private ThemeLanguageInfoProvider _themeLanguageInfoProvider;

    public MobileLanguageSwitchViewComponent(ThemeLanguageInfoProvider themeLanguageInfoProvider)
    {
        _themeLanguageInfoProvider = themeLanguageInfoProvider;
    }

    public virtual async Task<IViewComponentResult> InvokeAsync()
    {
        var languageInfo = await _themeLanguageInfoProvider.GetLanguageSwitchViewComponentModelAsync();
        return View("~/Themes/LeptonXLite/Components/Toolbar/MobileLanguageSwitch/Default.cshtml", languageInfo);
    }
}
