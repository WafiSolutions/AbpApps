using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Wafi.SmartHR.LeaveRecords.Dtos;

public class CreateUpdateLeaveRecordInput : IValidatableObject
{
    public Guid EmployeeId { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public LeaveType Type { get; set; }
    public string Reason { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        // If EndDate is default, treat it as same as StartDate — validation not needed.
        if (EndDate != default && StartDate > EndDate)
        {
            yield return new ValidationResult(
                "Start date cannot be after end date.",
                [nameof(StartDate), nameof(EndDate)]
            );
        }
    }
}
