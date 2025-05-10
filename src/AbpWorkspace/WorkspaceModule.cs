using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Modularity;

namespace Wafi.Abp.Workspace;

[DependsOn(typeof(AbpAspNetCoreMvcModule))]
public class WorkspaceModule : AbpModule
{
}
