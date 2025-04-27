using System;

namespace Wafi.SmartHR.LeaveRecords.Dtos;

public class CreateUpdateLeaveRecordDto
{
    public Guid EmployeeId { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string Reason { get; set; }
}
