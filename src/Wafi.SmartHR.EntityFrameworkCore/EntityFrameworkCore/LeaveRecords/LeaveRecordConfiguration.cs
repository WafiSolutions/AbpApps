using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Volo.Abp.EntityFrameworkCore.Modeling;
using Wafi.SmartHR.Employees;
using Wafi.SmartHR.LeaveRecords;

namespace Wafi.SmartHR.EntityFrameworkCore.EntityFrameworkCore.LeaveRecords
{
    public class LeaveRecordConfiguration : IEntityTypeConfiguration<LeaveRecord>
    {
        public void Configure(EntityTypeBuilder<LeaveRecord> builder)
        {
            builder.ToTable("LeaveRecords", SmartHRConsts.DbSchema);

            builder.ConfigureByConvention();

            builder.Property(x => x.Reason)
                .IsRequired()
                .HasMaxLength(512);

            builder.Property(x => x.Status)
                .IsRequired();

            builder.Property(x => x.TotalDays)
                .IsRequired();

            builder.HasOne<Employee>()
                .WithMany()
                .HasForeignKey(x => x.EmployeeId)
                .IsRequired();
        }
    }
} 