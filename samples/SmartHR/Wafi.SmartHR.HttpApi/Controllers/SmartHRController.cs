using Wafi.SmartHR.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace Wafi.SmartHR.Controllers;

/* Inherit your controllers from this class.
 */
public abstract class SmartHRController : AbpControllerBase
{
    protected SmartHRController()
    {
        LocalizationResource = typeof(SmartHRResource);
    }
}
