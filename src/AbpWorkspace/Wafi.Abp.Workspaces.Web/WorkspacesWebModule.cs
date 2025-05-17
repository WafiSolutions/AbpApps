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
        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<WorkspacesWebModule>();
        });

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

        Configure<AbpAutoMapperOptions>(options =>
        {
            options.AddMaps<WorkspacesWebModule>();
        });

        context.Services.AddAutoMapperObjectMapper<WorkspacesWebModule>();

        Configure<AbpBundlingOptions>(options =>
        {
            options.ScriptBundles.Configure(
                StandardBundles.Scripts.Global,
                bundle =>
                {
                    bundle.AddFiles("/js/workspace-constants.js");
                    bundle.AddFiles("/js/http-interceptor.js");
                    bundle.AddFiles("/js/workspace-selector.js");
                }
            );
        });
    }
}
