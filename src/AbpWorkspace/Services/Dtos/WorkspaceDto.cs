using Volo.Abp.Application.Dtos;

namespace Wafi.Abp.Workspaces.Services.Dtos;

public class WorkspaceDto : EntityDto<Guid>
{
    public string Name { get; set; }
}
