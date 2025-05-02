using Volo.Abp.Application.Dtos;

namespace Wafi.SmartHR.Employees.Dtos;

public class EmployeeFilter : PagedAndSortedResultRequestDto
{
    public string Filter { get; set; }
}