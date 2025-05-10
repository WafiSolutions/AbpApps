namespace Wafi.Abp.Workspace
{
    public interface ICurrentWorkspace
    {
        Guid? Id { get; }
        string Name { get; }
        IDisposable Change(Guid? id);
        IDisposable Change(Guid? id, string name);
    }
}
