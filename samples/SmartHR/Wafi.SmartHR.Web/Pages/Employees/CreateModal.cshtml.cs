using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Wafi.SmartHR.Employees;
using Wafi.SmartHR.Employees.Dtos;

namespace Wafi.SmartHR.Web.Pages.Employees;

public class CreateModalModel : SmartHRPageModel
{
    [BindProperty]
    public CreateUpdateEmployeeInput Employee { get; set; }

    private readonly IEmployeeAppService _employeeAppService;

    public CreateModalModel(IEmployeeAppService employeeAppService)
    {
        _employeeAppService = employeeAppService;
    }

    public void OnGet()
    {
        Employee = new CreateUpdateEmployeeInput();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        await _employeeAppService.CreateAsync(Employee);
        return NoContent();
    }
} 