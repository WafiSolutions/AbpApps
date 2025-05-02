using Volo.Abp.Application.Dtos;

namespace Wafi.SmartHR.LeaveRecords.Dtos;

public class LeaveRecordFilter : PagedAndSortedResultRequestDto
{
    public string Filter { get; set; }
}
