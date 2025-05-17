namespace Wafi.Abp.Workspaces;

public class WorkspaceResolveOptions
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
