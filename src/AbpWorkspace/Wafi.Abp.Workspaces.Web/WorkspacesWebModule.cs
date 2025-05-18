using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.Localization;
using Volo.Abp.AspNetCore.Mvc.UI.Bundling;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.Shared;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.Shared.Bundling;
using Volo.Abp.AutoMapper;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;
using Wafi.Abp.Workspaces.Localization;

namespace Wafi.Abp.Workspaces.Web;

[DependsOn(
    typeof(WorkspaceModule),
    typeof(AbpAspNetCoreMvcModule),
    typeof(AbpAutoMapperModule),
    typeof(AbpAspNetCoreMvcUiThemeSharedModule)
)]
public class WorkspacesWebModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        ConfigureVirtualFileSystem();
        ConfigureLocalization(context);
        ConfigureScriptBundling();
    }

    private void ConfigureVirtualFileSystem()
    {
        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<WorkspacesWebModule>();
            options.FileSets.AddEmbedded<WorkspacesWebModule>("Wafi.Abp.Workspaces.Web.wwwroot");
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
}
