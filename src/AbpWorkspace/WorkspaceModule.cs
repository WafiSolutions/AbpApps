using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Modularity;
using Wafi.Abp.Workspaces.Core;
using Wafi.Abp.Workspaces.EntityFrameworkCore;
using Wafi.Abp.Workspaces.Services;

namespace Wafi.Abp.Workspaces;

[DependsOn(typeof(AbpAspNetCoreMvcModule))]
public class WorkspaceModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
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
