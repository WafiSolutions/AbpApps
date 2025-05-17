using Volo.Abp.Application.Dtos;

namespace Wafi.Abp.Workspaces.Services.Dtos;

public class WorkspaceDto : AuditedEntityDto<Guid>
{
    public string Name { get; set; }
}
