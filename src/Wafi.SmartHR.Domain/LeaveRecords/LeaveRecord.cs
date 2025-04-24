using System;
using Volo.Abp.Domain.Entities.Auditing;

namespace Wafi.SmartHR.LeaveRecords
{
    public class LeaveRecord : FullAuditedAggregateRoot<Guid>
    {
        public Guid EmployeeId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public LeaveStatus Status { get; set; }
        public string Reason { get; set; }
        public int TotalDays { get; private set; }

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
            StartDate = startDate;
            EndDate = endDate;
            Reason = reason;
            Status = LeaveStatus.Pending;
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
} 