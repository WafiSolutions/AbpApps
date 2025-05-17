using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Wafi.Abp.Workspaces.Core;

namespace Wafi.Abp.Workspaces.Services;

/// <summary>
/// Implementation of the multi-workspace filter
/// </summary>
public class MultiWorkspaceFilter(
    ICurrentWorkspace CurrentWorkspace,
    IDataFilter DataFilter)
    : IMultiWorkspaceFilter, ITransientDependency
{
    protected readonly IDataFilter DataFilter = DataFilter;

    public bool IsEnabled => DataFilter.IsEnabled<IWorkspace>();
    public virtual bool IsWorkspaceEnabled => IsEnabled && CurrentWorkspace.Id.HasValue;
    public IDisposable Disable()
    {
        return DataFilter.Disable<IWorkspace>();
    }
    public IDisposable Enable()
    {
        return DataFilter.Enable<IWorkspace>();
    }

    public IQueryable<TEntity> ApplyFilter<TEntity>(IQueryable<TEntity> query)
        where TEntity : class, IWorkspace
    {
        if (!IsWorkspaceEnabled)
        {
            return query;
        }
        return query.Where(e => e.WorkspaceId == CurrentWorkspace.Id);
    }
    public Task ConfigureFilterAsync<TEntity>(IQueryable<TEntity> query)
        where TEntity : class, IWorkspace
    {
        // Implementation might not be needed for all cases, 
        // but provided for completeness with IMultiTenant pattern
        return Task.CompletedTask;
    }
}
