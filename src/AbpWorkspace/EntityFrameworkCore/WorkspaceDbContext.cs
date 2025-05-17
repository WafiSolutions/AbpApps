using Microsoft.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;
using Wafi.Abp.Workspaces.Core;

namespace Wafi.Abp.Workspaces.EntityFrameworkCore;

[ConnectionStringName("Default")]
public class WorkspaceDbContext :
    AbpDbContext<WorkspaceDbContext>
{
    public DbSet<Workspace> Workspace { get; set; }

    public WorkspaceDbContext(DbContextOptions<WorkspaceDbContext> options)
        : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ConfigureWorkspace();
    }
}
