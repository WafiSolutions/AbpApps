using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Modularity;

namespace Wafi.Abp.Workspaces;

[DependsOn(typeof(AbpAspNetCoreMvcModule))]
public class WorkspaceModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        ConfigureAutoAPIControllers();
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
