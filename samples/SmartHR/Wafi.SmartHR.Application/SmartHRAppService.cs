using Wafi.SmartHR.Localization;
using Volo.Abp.Application.Services;

namespace Wafi.SmartHR;

/* Inherit your application services from this class.
 */
public abstract class SmartHRAppService : ApplicationService
{
    protected SmartHRAppService()
    {
        LocalizationResource = typeof(SmartHRResource);
    }
}
