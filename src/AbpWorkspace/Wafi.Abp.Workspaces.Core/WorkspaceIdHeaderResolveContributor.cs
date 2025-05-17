using Microsoft.Extensions.Logging;
using Volo.Abp.DependencyInjection;

namespace Wafi.Abp.Workspaces;

/// <summary>
/// Resolves workspace from the X-Workspace-Id header in HTTP requests
/// </summary>
public class WorkspaceIdHeaderResolveContributor : IWorkspaceResolveContributor, ITransientDependency
{
    /// <summary>
    /// Default header name: X-Workspace-ID.
    /// </summary>
    public const string HeaderName = "X-Workspace-Id";

    /// <summary>
    /// Default contributor name: WorkspaceIdHeader.
    /// </summary>
    public const string ContributorName = "WorkspaceIdHeader";
    /// <summary>
    /// Name of the contributor.
    /// </summary>
    public string Name => ContributorName;
    private readonly ILogger<WorkspaceIdHeaderResolveContributor> _logger;
    public WorkspaceIdHeaderResolveContributor(ILogger<WorkspaceIdHeaderResolveContributor> logger)
    {
        _logger = logger;
    }
    /// <summary>
    /// Tries to resolve current Workspace from HTTP header.
    /// </summary>
    public Task ResolveAsync(IWorkspaceResolveContext context)
    {
        var httpContext = context.GetHttpContext();
        if (httpContext == null)
        {
            return Task.CompletedTask;
        }
        var WorkspaceIdHeader = httpContext.Request.Headers[HeaderName];
        if (WorkspaceIdHeader.Count == 0 || string.IsNullOrWhiteSpace(WorkspaceIdHeader[0]))
        {
            return Task.CompletedTask;
        }
        if (Guid.TryParse(WorkspaceIdHeader[0], out var workspaceId))
        {
            _logger.LogDebug($"Workspace Id found in request header: {workspaceId}");
            context.WorkspaceId = workspaceId;
        }
        else
        {
            _logger.LogDebug($"Invalid Workspace Id format in request header: {WorkspaceIdHeader[0]}");
        }
        return Task.CompletedTask;
    }
}
