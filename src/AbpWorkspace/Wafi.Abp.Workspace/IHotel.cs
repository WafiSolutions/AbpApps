using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wafi.Abp.Workspace
{
    public interface IWorkspace
    {
        Guid? WorkspaceId { get; set; }
    }
}
