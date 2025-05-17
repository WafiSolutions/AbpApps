using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
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

    public async Task<List<WorkspaceDto>> GetAllAsync()
    {
        var workspaces = await workspaceRepository.GetListAsync();
        return [.. workspaces.Select(w => new WorkspaceDto { Id = w.Id, Name = w.Name })];
    }

    public async Task DeleteAsync(Guid id)
    {
        await workspaceRepository.DeleteAsync(id);
    }
}
