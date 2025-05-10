using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wafi.Abp.Workspace
{
    class WorkspaceResolveOptions
    {
        public List<IWorkspaceResolveContributor> WorkspaceResolvers { get; }

        public WorkspaceResolveOptions()
        {
            WorkspaceResolvers = new List<IWorkspaceResolveContributor>();
        }

        public void AddResolver(IWorkspaceResolveContributor resolver)
        {
            WorkspaceResolvers.Add(resolver);
        }
    }
}
