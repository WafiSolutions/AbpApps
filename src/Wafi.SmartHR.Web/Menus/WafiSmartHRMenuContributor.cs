using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using Wafi.SmartHR.Localization;
using Wafi.SmartHR.MultiTenancy;
using Volo.Abp.TenantManagement.Web.Navigation;
using Volo.Abp.UI.Navigation;

namespace Wafi.SmartHR.Web.Menus
{
    public class WafiSmartHRMenuContributor : IMenuContributor
    {
        public async Task ConfigureMenuAsync(MenuConfigurationContext context)
        {
            if (context.Menu.Name == StandardMenus.Main)
            {
                await ConfigureMainMenuAsync(context);
            }
        }

        private async Task ConfigureMainMenuAsync(MenuConfigurationContext context)
        {
            if (!MultiTenancyConsts.IsEnabled)
            {
                var administration = context.Menu.GetAdministration();
                administration.TryRemoveMenuItem(TenantManagementMenuNames.GroupName);
            }

            var l = context.GetLocalizer<SmartHRResource>();

            context.Menu.Items.Insert(0, new ApplicationMenuItem("WafiSmartHR.Home", l["Menu:Home"], "~/"));
            
            context.Menu.AddItem(
                new ApplicationMenuItem(
                    "WafiSmartHR.Employees",
                    l["Menu:Employees"],
                    url: "/Employees"
                )
            );

            context.Menu.AddItem(
                new ApplicationMenuItem(
                    "WafiSmartHR.LeaveRecords",
                    l["Menu:LeaveRecords"],
                    url: "/LeaveRecords"
                )
            );
        }
    }
} 