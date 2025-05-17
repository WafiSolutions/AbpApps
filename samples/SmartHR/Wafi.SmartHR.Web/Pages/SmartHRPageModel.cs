using Wafi.SmartHR.Localization;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;

namespace Wafi.SmartHR.Web.Pages;

public abstract class SmartHRPageModel : AbpPageModel
{
    protected SmartHRPageModel()
    {
        LocalizationResourceType = typeof(SmartHRResource);
    }
}
