using System;
using Volo.Abp.Application.Dtos;

namespace Wafi.SmartHR.LeaveRecords.Dtos;

public class LeaveRecordDto : EntityDto<Guid>
{
    public string EmployeeName { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public LeaveStatus Status { get; set; }
    public string Reason { get; set; }
    public int TotalDays { get; set; }
}
