using System;
using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;
using Wafi.SmartHR.Employees;
using Wafi.SmartHR.LeaveRecords;

namespace Wafi.SmartHR.DbMigrator.Data;

public class EmployeeAndLeaveRecordDataSeeder : IDataSeedContributor, ITransientDependency
{
    private readonly IRepository<Employee, Guid> _employeeRepository;
    private readonly IRepository<LeaveRecord, Guid> _leaveRecordRepository;

    public EmployeeAndLeaveRecordDataSeeder(
        IRepository<Employee, Guid> employeeRepository,
        IRepository<LeaveRecord, Guid> leaveRecordRepository)
    {
        _employeeRepository = employeeRepository;
        _leaveRecordRepository = leaveRecordRepository;
    }

    public async Task SeedAsync(DataSeedContext context)
    {
        if (await _employeeRepository.GetCountAsync() > 0)
        {
            return;
        }

        // Create employees
        var employees = new[]
        {
            await _employeeRepository.InsertAsync(
                new Employee(
                    Guid.Parse("2e701e62-0953-4dd3-910b-dc6cc93ccb0d"),
                    "John",
                    "Doe",
                    "john.doe@example.com",
                    "1234567890",
                    new DateTime(1990, 1, 1),
                    new DateTime(2020, 1, 1),
                    20
                )
            ),
            await _employeeRepository.InsertAsync(
                new Employee(
                    Guid.Parse("2e701e62-0953-4dd3-910b-dc6cc93ccb0e"),
                    "Jane",
                    "Smith",
                    "jane.smith@example.com",
                    "0987654321",
                    new DateTime(1992, 5, 15),
                    new DateTime(2021, 3, 1),
                    25
                )
            ),
            await _employeeRepository.InsertAsync(
                new Employee(
                    Guid.Parse("2e701e62-0953-4dd3-910b-dc6cc93ccb0f"),
                    "Michael",
                    "Johnson",
                    "michael.johnson@example.com",
                    "1122334455",
                    new DateTime(1988, 7, 20),
                    new DateTime(2019, 6, 1),
                    30
                )
            ),
            await _employeeRepository.InsertAsync(
                new Employee(
                    Guid.Parse("2e701e62-0953-4dd3-910b-dc6cc93ccb10"),
                    "Sarah",
                    "Williams",
                    "sarah.williams@example.com",
                    "2233445566",
                    new DateTime(1991, 3, 10),
                    new DateTime(2020, 8, 15),
                    22
                )
            ),
            await _employeeRepository.InsertAsync(
                new Employee(
                    Guid.Parse("2e701e62-0953-4dd3-910b-dc6cc93ccb11"),
                    "David",
                    "Brown",
                    "david.brown@example.com",
                    "3344556677",
                    new DateTime(1989, 11, 25),
                    new DateTime(2021, 1, 10),
                    28
                )
            ),
            await _employeeRepository.InsertAsync(
                new Employee(
                    Guid.Parse("2e701e62-0953-4dd3-910b-dc6cc93ccb12"),
                    "Emily",
                    "Davis",
                    "emily.davis@example.com",
                    "4455667788",
                    new DateTime(1993, 9, 5),
                    new DateTime(2022, 2, 1),
                    18
                )
            ),
            await _employeeRepository.InsertAsync(
                new Employee(
                    Guid.Parse("2e701e62-0953-4dd3-910b-dc6cc93ccb13"),
                    "Robert",
                    "Miller",
                    "robert.miller@example.com",
                    "5566778899",
                    new DateTime(1987, 4, 30),
                    new DateTime(2018, 12, 1),
                    35
                )
            ),
            await _employeeRepository.InsertAsync(
                new Employee(
                    Guid.Parse("2e701e62-0953-4dd3-910b-dc6cc93ccb14"),
                    "Jennifer",
                    "Wilson",
                    "jennifer.wilson@example.com",
                    "6677889900",
                    new DateTime(1994, 2, 15),
                    new DateTime(2021, 7, 1),
                    20
                )
            ),
            await _employeeRepository.InsertAsync(
                new Employee(
                    Guid.Parse("2e701e62-0953-4dd3-910b-dc6cc93ccb15"),
                    "William",
                    "Moore",
                    "william.moore@example.com",
                    "7788990011",
                    new DateTime(1990, 8, 20),
                    new DateTime(2019, 9, 1),
                    25
                )
            ),
            await _employeeRepository.InsertAsync(
                new Employee(
                    Guid.Parse("2e701e62-0953-4dd3-910b-dc6cc93ccb16"),
                    "Elizabeth",
                    "Taylor",
                    "elizabeth.taylor@example.com",
                    "8899001122",
                    new DateTime(1992, 12, 10),
                    new DateTime(2020, 4, 1),
                    22
                )
            )
        };

        // Create leave records
        var leaveRecords = new[]
        {
            // John Doe's leaves
            new LeaveRecord(
                Guid.Parse("2e701e62-0953-4dd3-910b-dc6cc93ccb17"),
                employees[0].Id,
                new DateTime(2025, 1, 1),
                new DateTime(2025, 1, 5),
                "Annual leave"
            ),
            new LeaveRecord(
                Guid.Parse("2e701e62-0953-4dd3-910b-dc6cc93ccb18"),
                employees[0].Id,
                new DateTime(2025, 3, 15),
                new DateTime(2025, 3, 16),
                "Sick leave"
            ),
            new LeaveRecord(
                Guid.Parse("2e701e62-0953-4dd3-910b-dc6cc93ccb19"),
                employees[0].Id,
                new DateTime(2025, 6, 1),
                new DateTime(2025, 6, 3),
                "Personal leave"
            ),

            // Jane Smith's leaves
            new LeaveRecord(
                Guid.Parse("2e701e62-0953-4dd3-910b-dc6cc93ccb20"),
                employees[1].Id,
                new DateTime(2025, 2, 1),
                new DateTime(2025, 2, 3),
                "Sick leave"
            ),
            new LeaveRecord(
                Guid.Parse("2e701e62-0953-4dd3-910b-dc6cc93ccb21"),
                employees[1].Id,
                new DateTime(2025, 4, 10),
                new DateTime(2025, 4, 14),
                "Annual leave"
            ),
            new LeaveRecord(
                Guid.Parse("2e701e62-0953-4dd3-910b-dc6cc93ccb22"),
                employees[1].Id,
                new DateTime(2025, 7, 1),
                new DateTime(2025, 7, 7),
                "Vacation"
            ),

            // Michael Johnson's leaves
            new LeaveRecord(
                Guid.Parse("2e701e62-0953-4dd3-910b-dc6cc93ccb23"),
                employees[2].Id,
                new DateTime(2025, 1, 15),
                new DateTime(2025, 1, 20),
                "Annual leave"
            ),
            new LeaveRecord(
                Guid.Parse("2e701e62-0953-4dd3-910b-dc6cc93ccb24"),
                employees[2].Id,
                new DateTime(2025, 3, 1),
                new DateTime(2025, 3, 2),
                "Sick leave"
            ),

            // Sarah Williams' leaves
            new LeaveRecord(
                Guid.Parse("2e701e62-0953-4dd3-910b-dc6cc93ccb25"),
                employees[3].Id,
                new DateTime(2025, 2, 10),
                new DateTime(2025, 2, 12),
                "Personal leave"
            ),
            new LeaveRecord(
                Guid.Parse("2e701e62-0953-4dd3-910b-dc6cc93ccb26"),
                employees[3].Id,
                new DateTime(2025, 5, 1),
                new DateTime(2025, 5, 5),
                "Annual leave"
            ),

            // David Brown's leaves
            new LeaveRecord(
                Guid.Parse("2e701e62-0953-4dd3-910b-dc6cc93ccb27"),
                employees[4].Id,
                new DateTime(2025, 1, 5),
                new DateTime(2025, 1, 6),
                "Sick leave"
            ),
            new LeaveRecord(
                Guid.Parse("2e701e62-0953-4dd3-910b-dc6cc93ccb28"),
                employees[4].Id,
                new DateTime(2025, 4, 1),
                new DateTime(2025, 4, 7),
                "Annual leave"
            ),

            // Emily Davis' leaves
            new LeaveRecord(
                Guid.Parse("2e701e62-0953-4dd3-910b-dc6cc93ccb29"),
                employees[5].Id,
                new DateTime(2025, 3, 1),
                new DateTime(2025, 3, 3),
                "Sick leave"
            ),
            new LeaveRecord(
                Guid.Parse("2e701e62-0953-4dd3-910b-dc6cc93ccb30"),
                employees[5].Id,
                new DateTime(2025, 6, 1),
                new DateTime(2025, 6, 4),
                "Personal leave"
            ),

            // Robert Miller's leaves
            new LeaveRecord(
                Guid.Parse("2e701e62-0953-4dd3-910b-dc6cc93ccb31"),
                employees[6].Id,
                new DateTime(2025, 2, 1),
                new DateTime(2025, 2, 5),
                "Annual leave"
            ),
            new LeaveRecord(
                Guid.Parse("2e701e62-0953-4dd3-910b-dc6cc93ccb32"),
                employees[6].Id,
                new DateTime(2025, 5, 1),
                new DateTime(2025, 5, 2),
                "Sick leave"
            ),

            // Jennifer Wilson's leaves
            new LeaveRecord(
                Guid.Parse("2e701e62-0953-4dd3-910b-dc6cc93ccb33"),
                employees[7].Id,
                new DateTime(2025, 1, 10),
                new DateTime(2025, 1, 12),
                "Personal leave"
            ),
            new LeaveRecord(
                Guid.Parse("2e701e62-0953-4dd3-910b-dc6cc93ccb34"),
                employees[7].Id,
                new DateTime(2025, 4, 1),
                new DateTime(2025, 4, 4),
                "Annual leave"
            ),

            // William Moore's leaves
            new LeaveRecord(
                Guid.Parse("2e701e62-0953-4dd3-910b-dc6cc93ccb35"),
                employees[8].Id,
                new DateTime(2025, 3, 1),
                new DateTime(2025, 3, 5),
                "Annual leave"
            ),
            new LeaveRecord(
                Guid.Parse("2e701e62-0953-4dd3-910b-dc6cc93ccb36"),
                employees[8].Id,
                new DateTime(2025, 6, 1),
                new DateTime(2025, 6, 2),
                "Sick leave"
            ),

            // Elizabeth Taylor's leaves
            new LeaveRecord(
                Guid.Parse("2e701e62-0953-4dd3-910b-dc6cc93ccb37"),
                employees[9].Id,
                new DateTime(2025, 2, 1),
                new DateTime(2025, 2, 3),
                "Personal leave"
            ),
            new LeaveRecord(
                Guid.Parse("2e701e62-0953-4dd3-910b-dc6cc93ccb38"),
                employees[9].Id,
                new DateTime(2025, 5, 1),
                new DateTime(2025, 5, 5),
                "Annual leave"
            )
        };

        // Insert all leave records
        foreach (var leaveRecord in leaveRecords)
        {
            await _leaveRecordRepository.InsertAsync(leaveRecord);
        }
    }
}
