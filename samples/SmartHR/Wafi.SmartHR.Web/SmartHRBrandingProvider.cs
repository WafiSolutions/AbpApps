using Volo.Abp.Ui.Branding;
using Volo.Abp.DependencyInjection;
using Microsoft.Extensions.Localization;
using Wafi.SmartHR.Localization;

namespace Wafi.SmartHR.Web;

[Dependency(ReplaceServices = true)]
public class SmartHRBrandingProvider : DefaultBrandingProvider
{
    private IStringLocalizer<SmartHRResource> _localizer;

    public SmartHRBrandingProvider(IStringLocalizer<SmartHRResource> localizer)
    {
        _localizer = localizer;
    }

    public override string AppName => _localizer["AppName"];
}
