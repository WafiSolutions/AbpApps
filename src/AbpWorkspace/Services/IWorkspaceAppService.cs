using Volo.Abp.Application.Services;
using Wafi.Abp.Workspaces.Services.Dtos;

namespace Wafi.Abp.Workspaces.Services;

public interface IWorkspaceAppService : IApplicationService
{
    Task<WorkspaceDto> CreateAsync(WorkspaceDto input);
    Task<WorkspaceDto> UpdateAsync(Guid id, WorkspaceDto input);
    Task<WorkspaceDto> GetAsync(Guid id);
    Task<List<WorkspaceDto>> GetAllAsync();
    Task DeleteAsync(Guid id);
}
