using Workspace.Sample.Localization;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;

namespace Workspace.Sample.Web.Pages;

public abstract class SamplePageModel : AbpPageModel
{
    protected SamplePageModel()
    {
        LocalizationResourceType = typeof(SampleResource);
    }
}
