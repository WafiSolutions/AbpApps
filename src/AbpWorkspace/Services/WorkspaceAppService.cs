using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Wafi.Abp.Workspaces.Core;
using Wafi.Abp.Workspaces.Services.Dtos;

namespace Wafi.Abp.Workspaces.Services;

public class WorkspaceAppService : ApplicationService, IWorkspaceAppService
{
    private readonly IRepository<Workspace, Guid> _workspaceRepository;

    public WorkspaceAppService(IRepository<Workspace, Guid> workspaceRepository)
    {
        _workspaceRepository = workspaceRepository;
    }

    public async Task<WorkspaceDto> CreateAsync(WorkspaceDto input)
    {
        var workspace = new Workspace { Name = input.Name };
        var createdWorkspace = await _workspaceRepository.InsertAsync(workspace);
        return new WorkspaceDto { Id = createdWorkspace.Id, Name = createdWorkspace.Name };
    }

    public async Task<WorkspaceDto> UpdateAsync(Guid id, WorkspaceDto input)
    {
        var workspace = await _workspaceRepository.GetAsync(id);
        workspace.Name = input.Name;
        var updatedWorkspace = await _workspaceRepository.UpdateAsync(workspace);
        return new WorkspaceDto { Id = updatedWorkspace.Id, Name = updatedWorkspace.Name };
    }

    public async Task<WorkspaceDto> GetAsync(Guid id)
    {
        var workspace = await _workspaceRepository.GetAsync(id);
        return new WorkspaceDto { Id = workspace.Id, Name = workspace.Name };
    }

    public async Task<List<WorkspaceDto>> GetAllAsync()
    {
        var workspaces = await _workspaceRepository.GetListAsync();
        return [.. workspaces.Select(w => new WorkspaceDto { Id = w.Id, Name = w.Name })];
    }

    public async Task DeleteAsync(Guid id)
    {
        await _workspaceRepository.DeleteAsync(id);
    }
}
