using Microsoft.EntityFrameworkCore;
using Volo.Abp;
using Volo.Abp.EntityFrameworkCore.Modeling;
using Wafi.SmartHR.Employees;
using Wafi.SmartHR.LeaveRecords;

namespace Wafi.SmartHR.EntityFrameworkCore;

public static class SmartHRDbContextModelCreatingExtensions
{
    public static void ConfigureSmartHR(
        this ModelBuilder builder)
    {
        Check.NotNull(builder, nameof(builder));


        builder.Entity<Employee>(b =>
        {
            b.ToTable(SmartHRDbProperties.DbTablePrefix + "Employees", SmartHRDbProperties.DbSchema);
            b.ConfigureByConvention();
        });

        builder.Entity<LeaveRecord>(b =>
        {
            b.ToTable(SmartHRDbProperties.DbTablePrefix + "LeaveRecords", SmartHRDbProperties.DbSchema);
            b.ConfigureByConvention();
        });

    }
}
