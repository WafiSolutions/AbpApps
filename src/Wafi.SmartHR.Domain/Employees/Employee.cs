using System;
using Volo.Abp.Domain.Entities.Auditing;

namespace Wafi.SmartHR.Employees
{
    public class Employee : FullAuditedAggregateRoot<Guid>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime DateOfBirth { get; set; }
        public DateTime JoiningDate { get; set; }
        public int TotalLeaveDays { get; set; }
        public int RemainingLeaveDays { get; set; }

        protected Employee()
        {
            // Required by EF Core
        }

        public Employee(
            Guid id,
            string firstName,
            string lastName,
            string email,
            string phoneNumber,
            DateTime dateOfBirth,
            DateTime joiningDate,
            int totalLeaveDays
        ) : base(id)
        {
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            PhoneNumber = phoneNumber;
            DateOfBirth = dateOfBirth;
            JoiningDate = joiningDate;
            TotalLeaveDays = totalLeaveDays;
            RemainingLeaveDays = totalLeaveDays;
        }

        public void UpdateRemainingLeaveDays(int daysUsed)
        {
            if (daysUsed > RemainingLeaveDays)
            {
                throw new Exception("Not enough remaining leave days");
            }
            RemainingLeaveDays -= daysUsed;
        }
    }
} 