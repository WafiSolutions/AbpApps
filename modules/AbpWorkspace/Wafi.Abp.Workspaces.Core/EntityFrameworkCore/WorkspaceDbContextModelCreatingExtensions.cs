using Microsoft.EntityFrameworkCore;
using Volo.Abp;
using Volo.Abp.EntityFrameworkCore.Modeling;
using Wafi.Abp.Workspaces.Core;

namespace Wafi.Abp.Workspaces.EntityFrameworkCore;

public static class WorkspaceDbContextModelCreatingExtensions
{
    public static void ConfigureWorkspace(
        this ModelBuilder builder)
    {
        Check.NotNull(builder, nameof(builder));


        builder.Entity<Workspace>(b =>
        {
            b.ToTable(WorkspaceDbProperties.DbTablePrefix + "Workspaces", WorkspaceDbProperties.DbSchema);
            b.ConfigureByConvention();
        });

    }
}

