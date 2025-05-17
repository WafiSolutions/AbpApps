using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Modularity;
using Wafi.Abp.Workspaces.EntityFrameworkCore;

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

        // Configure hotel resolver options
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
