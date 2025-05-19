using System.Reflection;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.Localization;
using Volo.Abp.AspNetCore.Mvc.UI.Bundling;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.LeptonXLite;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.Shared;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.Shared.Bundling;
using Volo.Abp.AutoMapper;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.UI.Navigation;
using Volo.Abp.VirtualFileSystem;
using Wafi.Abp.Workspaces.Localization;
using Wafi.Abp.Workspaces.Web.Menus;

namespace Wafi.Abp.Workspaces.Web;

[DependsOn(
    typeof(WorkspaceModule),
    typeof(AbpAspNetCoreMvcModule),
    typeof(AbpAutoMapperModule),
    typeof(AbpAspNetCoreMvcUiThemeSharedModule),
    typeof(AbpAspNetCoreMvcUiLeptonXLiteThemeModule)
)]
public class WorkspacesWebModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        ConfigureVirtualFileSystem();
        ConfigureLocalization(context);
        ConfigureScriptBundling();
        ConfigureMvc();
        ConfigureRazorPages(context);
    }

    private void ConfigureVirtualFileSystem()
    {
        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<WorkspacesWebModule>();
            options.FileSets.AddEmbedded<WorkspacesWebModule>("Wafi.Abp.Workspaces.Web.wwwroot");
            options.FileSets.AddEmbedded<WorkspacesWebModule>("Wafi.Abp.Workspaces.Web.Pages");
        });
    }

    private void ConfigureLocalization(ServiceConfigurationContext context)
    {
        context.Services.PreConfigure<AbpMvcDataAnnotationsLocalizationOptions>(options =>
        {
            options.AddAssemblyResource(typeof(WorkspaceResource), typeof(WorkspacesWebModule).Assembly);
        });

        Configure<AbpLocalizationOptions>(options =>
        {
            options.Resources
                .Get<WorkspaceResource>()
                .AddVirtualJson("/Localization/Resources");
        });
    }

    private void ConfigureScriptBundling()
    {
        Configure<AbpBundlingOptions>(options =>
        {
            options.ScriptBundles.Configure(
                StandardBundles.Scripts.Global,
                bundle =>
                {
                    bundle.AddFiles(
                        "/js/workspace-constants.js",
                        "/js/http-interceptor.js",
                        "/js/workspace-selector.js"
                    );
                });
        });
    }

    private void ConfigureMvc()
    {
        Configure<AbpAspNetCoreMvcOptions>(options =>
        {
            // Create conventional controllers for this assembly
            options.ConventionalControllers.Create(typeof(WorkspacesWebModule).Assembly);
        });
    }

    private void ConfigureRazorPages(ServiceConfigurationContext context)
    {
        context.Services.Configure<AbpAspNetCoreMvcOptions>(options =>
        {
            options.ConventionalControllers.Create(typeof(WorkspacesWebModule).Assembly);
        });
    }
}
