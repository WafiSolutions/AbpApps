using Volo.Abp.Domain.Entities.Auditing;

namespace Wafi.Abp.Workspace.Core;

public class Workspace : AuditedAggregateRoot<Guid>
{
    public string Name { get; set; }
}
