using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wafi.Abp.Workspaces
{
    public class WorkspaceResolveContext : IWorkspaceResolveContext
    {
        public Guid? WorkspaceId { get; set; }
        public string WorkspaceName { get; set; }

        public IServiceProvider ServiceProvider => _httpContext.RequestServices;

        private readonly HttpContext _httpContext;
        public WorkspaceResolveContext(HttpContext httpContext)
        {
            _httpContext = httpContext;
        }
        public HttpContext GetHttpContext()
        {
            return _httpContext;
        }
    }
}
