using System;

namespace Wafi.SmartHR.Employees.Dtos;

public class CreateUpdateEmployeeInput
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public DateTime DateOfBirth { get; set; }
    public DateTime JoiningDate { get; set; }
    public int TotalLeaveDays { get; set; }
}