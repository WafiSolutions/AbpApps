using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Volo.Abp.EntityFrameworkCore.Modeling;
using Wafi.SmartHR.Employees;

namespace Wafi.SmartHR.EntityFrameworkCore.EntityFrameworkCore.Employees
{
    public class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
    {
        public void Configure(EntityTypeBuilder<Employee> builder)
        {
            builder.ToTable("Employees", SmartHRConsts.DbSchema);

            builder.ConfigureByConvention();

            builder.Property(x => x.FirstName)
                .IsRequired()
                .HasMaxLength(128);

            builder.Property(x => x.LastName)
                .IsRequired()
                .HasMaxLength(128);

            builder.Property(x => x.Email)
                .IsRequired()
                .HasMaxLength(256);

            builder.Property(x => x.PhoneNumber)
                .HasMaxLength(32);

            builder.Property(x => x.TotalLeaveDays)
                .IsRequired();

            builder.Property(x => x.RemainingLeaveDays)
                .IsRequired();
        }
    }
} 