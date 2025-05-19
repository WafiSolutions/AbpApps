using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Wafi.SmartHR.Employees;
using Wafi.SmartHR.Employees.Dtos;

namespace Wafi.SmartHR.Web.Pages.Employees;

public class EditModalModel : SmartHRPageModel
{
    [HiddenInput]
    [BindProperty(SupportsGet = true)]
    public Guid Id { get; set; }

    [BindProperty]
    public CreateUpdateEmployeeInput Employee { get; set; }
    
    public int RemainingLeaveDays { get; set; }

    private readonly IEmployeeAppService _employeeAppService;

    public EditModalModel(IEmployeeAppService employeeAppService)
    {
        _employeeAppService = employeeAppService;
    }

    public async Task OnGetAsync()
    {
        var employee = await _employeeAppService.GetAsync(Id);
        
        Employee = new CreateUpdateEmployeeInput
        {
            FirstName = employee.FirstName,
            LastName = employee.LastName,
            Email = employee.Email,
            PhoneNumber = employee.PhoneNumber,
            DateOfBirth = employee.DateOfBirth,
            JoiningDate = employee.JoiningDate,
            TotalLeaveDays = employee.TotalLeaveDays
        };
        
        RemainingLeaveDays = employee.RemainingLeaveDays;
    }

    public async Task<IActionResult> OnPostAsync()
    {
        await _employeeAppService.UpdateAsync(Id, Employee);
        return NoContent();
    }
} 