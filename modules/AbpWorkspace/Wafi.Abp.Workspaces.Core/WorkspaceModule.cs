using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Localization;
using Volo.Abp.Localization.ExceptionHandling;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;
using Wafi.Abp.Workspaces.Core;
using Wafi.Abp.Workspaces.EntityFrameworkCore;
using Wafi.Abp.Workspaces.Localization;
using Wafi.Abp.Workspaces.Services;

namespace Wafi.Abp.Workspaces;

[DependsOn(typeof(AbpAspNetCoreMvcModule))]
public class WorkspaceModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        ConfigureLocalization();
        ConfigureAutoAPIControllers();

        context.Services.AddAbpDbContext<WorkspaceDbContext>(options =>
        {
            options.AddDefaultRepositories();
        });

        // Configure workspace resolver options
        Configure<WorkspaceResolveOptions>(options =>
        {
            options.AddResolver(context.Services.GetRequiredService<WorkspaceIdHeaderResolveContributor>());
        });
    }

    private void ConfigureLocalization()
    {
        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<WorkspaceModule>();
        });

        Configure<AbpLocalizationOptions>(options =>
        {
            options.Resources
                .Add<WorkspaceResource>("en")
                .AddVirtualJson("/Localization/Resources");
        });

        Configure<AbpExceptionLocalizationOptions>(options =>
        {
            options.MapCodeNamespace("Workspace", typeof(WorkspaceResource));
        });
    }

    private void ConfigureAutoAPIControllers()
    {
        Configure<AbpAspNetCoreMvcOptions>(options =>
        {
            options.ConventionalControllers.Create(typeof(WorkspaceModule).Assembly, opts =>
            {
                opts.RootPath = "Workspace";
            });
        });
    }
}
