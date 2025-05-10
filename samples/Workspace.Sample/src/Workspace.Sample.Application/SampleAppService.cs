using Workspace.Sample.Localization;
using Volo.Abp.Application.Services;
using Wafi.Abp.Workspace;

namespace Workspace.Sample;

/* Inherit your application services from this class.
 */
public abstract class SampleAppService : ApplicationService
{
    protected ICurrentWorkspace CurrentWorkspace => LazyServiceProvider.LazyGetRequiredService<ICurrentWorkspace>();
    protected SampleAppService()
    {
        LocalizationResource = typeof(SampleResource);
    }
}
