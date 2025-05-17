using System;
using Volo.Abp.Domain.Entities.Auditing;
using Wafi.SmartHR.Employees;

namespace Wafi.SmartHR.LeaveRecords;

public class LeaveRecord : FullAuditedAggregateRoot<Guid>
{
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public LeaveStatus Status { get; set; }
    public LeaveType Type { get; set; }
    public string Reason { get; set; }
    public int TotalDays { get; private set; }

    public Guid EmployeeId { get; set; }
    public Employee Employee { get; set; }

    protected LeaveRecord()
    {
        // Required by EF Core
    }

    public LeaveRecord(
        Guid id,
        Guid employeeId,
        DateTime startDate,
        DateTime endDate,
        string reason
    ) : base(id)
    {
        EmployeeId = employeeId;
        Reason = reason;
        Status = LeaveStatus.Pending;
        SetLeaveDate(startDate, endDate);
    }

    private void SetLeaveDate(DateTime startDate, DateTime endDate)
    {
        // Validate: StartDate should not be after EndDate
        if (StartDate > EndDate)
            throw new ArgumentException("Start date cannot be after end date.");

        StartDate = startDate;

        // If endDate is not provided (is default), treat it as a single-day leave
        EndDate = endDate == default || endDate == DateTime.MinValue
            ? startDate
            : endDate;

        CalculateTotalDays();
    }

    private void CalculateTotalDays()
    {
        TotalDays = (EndDate - StartDate).Days + 1; // Including both start and end dates
    }

    public void UpdateStatus(LeaveStatus newStatus)
    {
        Status = newStatus;
    }
}
