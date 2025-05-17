using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.UI.Alerts;

namespace Volo.Abp.AspNetCore.Mvc.UI.Theme.LeptonXLite.Themes.LeptonXLite.Components.PageAlerts;

public class PageAlertsViewComponent : AbpViewComponent
{
    protected IAlertManager AlertManager { get; }

    public PageAlertsViewComponent(IAlertManager alertManager)
    {
        AlertManager = alertManager;
    }

    public virtual IViewComponentResult Invoke(string name)
    {
        return View("~/Themes/LeptonXLite/Components/PageAlerts/Default.cshtml", AlertManager.Alerts);
    }
}
