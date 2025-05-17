using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Volo.Abp;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Wafi.Abp.Workspace.Core;

public static class WorkspaceDbContextModelCreatingExtensions
{
    public static void ConfigureWorkspace(
        this ModelBuilder builder)
    {
        Check.NotNull(builder, nameof(builder));


        builder.Entity<Workspace>(b =>
        {
            b.ToTable(SmartHRDbProperties.DbTablePrefix + "Workspaces", SmartHRDbProperties.DbSchema);
            b.ConfigureByConvention();
        });

    }
}

