using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Volo.Abp.AspNetCore.Mvc.UI.Layout;

namespace Volo.Abp.AspNetCore.Mvc.UI.Theme.LeptonXLite.Themes.LeptonXLite.Components.BreadCrumbs
{
    public class BreadCrumbsViewComponent : AbpViewComponent
    {
        protected IPageLayout PageLayout { get; }

        public BreadCrumbsViewComponent(IPageLayout pageLayout)
        {
            PageLayout = pageLayout;
        }

        public virtual async Task<IViewComponentResult> InvokeAsync()
        {
            return View("~/Themes/LeptonXLite/Components/BreadCrumbs/Default.cshtml", PageLayout.Content);
        }
    }
}
