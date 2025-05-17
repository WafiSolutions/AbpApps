using Microsoft.EntityFrameworkCore;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using System.Linq.Dynamic.Core;
using Wafi.Abp.Workspaces.Core;
using Wafi.Abp.Workspaces.Services.Dtos;

namespace Wafi.Abp.Workspaces.Services;

public class WorkspaceAppService(IRepository<Workspace, Guid> workspaceRepository) 
    : ApplicationService, IWorkspaceAppService
{
    public async Task<WorkspaceDto> CreateAsync(string name)
    {
        var workspace = new Workspace { Name = name };

        var createdWorkspace = await workspaceRepository.InsertAsync(workspace);

        return new WorkspaceDto { Id = createdWorkspace.Id, Name = createdWorkspace.Name };
    }

    public async Task<WorkspaceDto> UpdateAsync(Guid id, string name)
    {
        var workspace = await workspaceRepository.GetAsync(id);
        workspace.Name = name;

        var updatedWorkspace = await workspaceRepository.UpdateAsync(workspace);
        return new WorkspaceDto { Id = updatedWorkspace.Id, Name = updatedWorkspace.Name };
    }

    public async Task<WorkspaceDto> GetAsync(Guid id)
    {
        var workspace = await workspaceRepository.GetAsync(id);
        return new WorkspaceDto { Id = workspace.Id, Name = workspace.Name };
    }

    public async Task<PagedResultDto<WorkspaceDto>> GetAllAsync(PagedAndSortedResultRequestDto filter)
    {
        string sortBy = !string.IsNullOrWhiteSpace(filter.Sorting) ? filter.Sorting : nameof(Workspace.CreationTime);

        var workspaceQueryable = (await workspaceRepository.GetQueryableAsync()).AsNoTracking();

        var totalCount = await workspaceQueryable.CountAsync();

        var result = await (from e in workspaceQueryable 
                            select new WorkspaceDto
                            {
                                Id = e.Id,
                                Name = e.Name
                            }).OrderBy(sortBy).PageBy(filter).ToListAsync();

        return new PagedResultDto<WorkspaceDto>(
            totalCount,
            result
        );
    }

    public async Task DeleteAsync(Guid id)
    {
        await workspaceRepository.DeleteAsync(id);
    }
}
