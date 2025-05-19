using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wafi.Abp.Workspaces
{
    public interface IWorkspaceResolveContributor
    {
        string Name { get; }
        Task ResolveAsync(IWorkspaceResolveContext context);
    }
}
