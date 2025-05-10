using Volo.Abp.Ui.Branding;
using Volo.Abp.DependencyInjection;
using Microsoft.Extensions.Localization;
using Workspace.Sample.Localization;

namespace Workspace.Sample.Web;

[Dependency(ReplaceServices = true)]
public class SampleBrandingProvider : DefaultBrandingProvider
{
    private IStringLocalizer<SampleResource> _localizer;

    public SampleBrandingProvider(IStringLocalizer<SampleResource> localizer)
    {
        _localizer = localizer;
    }

    public override string AppName => _localizer["AppName"];
}
