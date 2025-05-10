using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace Wafi.Abp.Workspace
{
    public class WorkspaceResolutionMiddleware : IMiddleware, ITransientDependency
    {
        private readonly ILogger<WorkspaceResolutionMiddleware> _logger;
        private readonly WorkspaceResolveOptions _options;
        public WorkspaceResolutionMiddleware(
            IOptions<WorkspaceResolveOptions> options,
            ILogger<WorkspaceResolutionMiddleware> logger)
        {
            _logger = logger;
            _options = options.Value;
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
                var currentWorkspace = context.RequestServices.GetRequiredService<ICurrentWorkspace>();
                using (currentWorkspace.Change(workspaceResolveContext.WorkspaceId.Value, workspaceResolveContext.WorkspaceName))
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

}
