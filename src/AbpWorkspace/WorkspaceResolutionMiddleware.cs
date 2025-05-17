using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Volo.Abp.DependencyInjection;
using Wafi.Abp.Workspaces.Services;

namespace Wafi.Abp.Workspaces;

public class WorkspaceResolutionMiddleware : IMiddleware, ITransientDependency
{
    private readonly ILogger<WorkspaceResolutionMiddleware> _logger;
    private readonly WorkspaceResolveOptions _options;
    private readonly ICurrentWorkspace _currentWorkspace;
    public WorkspaceResolutionMiddleware(
        IOptions<WorkspaceResolveOptions> options,
        ILogger<WorkspaceResolutionMiddleware> logger,
        ICurrentWorkspace currentWorkspace)
    {
        _logger = logger;
        _options = options.Value;
        _currentWorkspace = currentWorkspace;
    }
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        var workspaceResolveContext = new WorkspaceResolveContext(context);

        foreach (var workspaceResolver in _options.WorkspaceResolvers)
        {
            await workspaceResolver.ResolveAsync(workspaceResolveContext);

            if (workspaceResolveContext.WorkspaceId.HasValue)
            {
                _logger.LogDebug($"Workspace resolved by {workspaceResolver.Name}: {workspaceResolveContext.WorkspaceId}");
                break;
            }
        }
        if (workspaceResolveContext.WorkspaceId.HasValue)
        {
            // Set current workspace using scoped ICurrentWorkspace service 
            using (_currentWorkspace.Change(workspaceResolveContext.WorkspaceId.Value, workspaceResolveContext.WorkspaceName))
            {
                await next(context);
            }
        }
        else
        {
            await next(context);
        }
    }
}

