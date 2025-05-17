using System.Threading.Tasks;
using Wafi.SmartHR.Localization;
using Wafi.SmartHR.Permissions;
using Wafi.SmartHR.MultiTenancy;
using Volo.Abp.SettingManagement.Web.Navigation;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Identity.Web.Navigation;
using Volo.Abp.UI.Navigation;
using Volo.Abp.TenantManagement.Web.Navigation;
using Volo.Abp.Account.Localization;
using Microsoft.Extensions.Configuration;
using System;
using Volo.Abp.AspNetCore.Mvc.UI.Theming;

namespace Wafi.SmartHR.Web.Menus;

public class SmartHRMenuContributor(IConfiguration configuration) : IMenuContributor
{
    public async Task ConfigureMenuAsync(MenuConfigurationContext context)
    {
        if (context.Menu.Name == StandardMenus.Main)
        {
            await ConfigureMainMenuAsync(context);
            await ConfigureAdminMenuAsync(context);
        }
        else if (context.Menu.Name == StandardMenus.User)
        {
            await ConfigureUserMenuAsync(context);
        }
    }

    private static Task ConfigureMainMenuAsync(MenuConfigurationContext context)
    {
        var l = context.GetLocalizer<SmartHRResource>();

        //Home
        context.Menu.AddItem(
            new ApplicationMenuItem(
                SmartHRMenus.Home,
                l["Menu:Home"],
                "~/",
                icon: "fa fa-home",
                order: 1
            )
        );

        //Employees
        context.Menu.AddItem(
            new ApplicationMenuItem(
                SmartHRMenus.Employees,
                l["Menu:Employees"],
                url: "/Employees",
                icon: "fa fa-users",
                order: 2
            ).RequirePermissions(SmartHRPermissions.Employees.Default)
        );

        //Leave Records
        context.Menu.AddItem(
            new ApplicationMenuItem(
                SmartHRMenus.LeaveRecords,
                l["Menu:LeaveRecords"],
                url: "/LeaveRecords",
                icon: "fa fa-calendar",
                order: 3
            ).RequirePermissions(SmartHRPermissions.LeaveRecords.Default)
        );


        //Leave Records
        context.Menu.AddItem(
            new ApplicationMenuItem(
                SmartHRMenus.LeaveRecords,
                l["Chat"],
                url: "/chat",
                icon: "fa fa-comments",
                order: 4
            ).RequirePermissions(SmartHRPermissions.LeaveRecords.Default)
        );

        //Administration
        var administration = context.Menu.GetAdministration();
        administration.Order = 6;

        //Administration->Identity
        administration.SetSubItemOrder(IdentityMenuNames.GroupName, 1);

        if (MultiTenancyConsts.IsEnabled)
        {
            administration.SetSubItemOrder(TenantManagementMenuNames.GroupName, 1);
        }
        else
        {
            administration.TryRemoveMenuItem(TenantManagementMenuNames.GroupName);
        }

        administration.SetSubItemOrder(SettingManagementMenuNames.GroupName, 3);

        //Administration->Settings
        administration.SetSubItemOrder(SettingManagementMenuNames.GroupName, 7);

        return Task.CompletedTask;
    }

    private Task ConfigureAdminMenuAsync(MenuConfigurationContext context)
    {
        var l = context.GetLocalizer<SmartHRResource>();

        //Add main menu items.
        var administration = context.Menu.GetAdministration();
        administration.AddItem(
            new ApplicationMenuItem(
                "Workspaces",
                l["Workspaces"],
                "/workspaces",
                icon: "fa fa-briefcase",
                order: 5
            )
        );

        return Task.CompletedTask;
    }

    private Task ConfigureUserMenuAsync(MenuConfigurationContext context)
    {
        var l = context.GetLocalizer<SmartHRResource>();
        var accountStringLocalizer = context.GetLocalizer<AccountResource>();
        var identityServerUrl = configuration["AuthServer:Authority"] ?? "";

        //context.Menu.AddItem(new ApplicationMenuItem("Account.Manage", accountStringLocalizer["MyAccount"],
        //    $"{identityServerUrl.EnsureEndsWith('/')}Account/Manage?returnUrl={configuration["App:SelfUrl"]}", icon: "bi bi-gear", order: 1000, null, "_blank").RequireAuthenticated());
        //context.Menu.AddItem(new ApplicationMenuItem("Account.Logout", l["Logout"], url: "~/Account/Logout", icon: "fa fa-power-off", order: int.MaxValue - 1000).RequireAuthenticated());

        return Task.CompletedTask;
    }
}
