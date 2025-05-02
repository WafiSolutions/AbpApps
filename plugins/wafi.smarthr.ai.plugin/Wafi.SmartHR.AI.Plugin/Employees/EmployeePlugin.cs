using System;
using System.ComponentModel;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.SemanticKernel;
using Volo.Abp.Application.Services;
using Volo.Abp.DependencyInjection;
using Wafi.SmartHR.Employees;
using Wafi.SmartHR.Employees.Dtos;
using Wafi.SmartHR.Permissions;

namespace Wafi.SmartHR.AI.Plugin.Employees;

public class EmployeePlugin(IEmployeeAppService employeeService, IAuthorizationService authorizationService)
    : ApplicationService, ITransientDependency
{
    [KernelFunction, Description("Get employee list")]
    public async Task<string> GetEmployeesAsync()
    {
        if (!await authorizationService.IsGrantedAsync(SmartHRPermissions.Employees.Default))
        {
            return "You are not authorized to access employee records";
        }

        var result = await employeeService.GetListAsync();
        return JsonSerializer.Serialize(result);
    }


    [KernelFunction, Description("Get the employee ID and detail from their name, email, PhoneNumber")]
    public async Task<string> GetEmployeeDetailsAsync(string filter)
    {
        var employees = await employeeService.GetPagedListAsync(new EmployeeFilter() { Filter = filter });
        if (employees is null)
        {
            return $"Employee with name '{filter}' not found.";
        }

        return JsonSerializer.Serialize(employees);
    }


    [KernelFunction, Description("Create a new employee")]
    public async Task<string> CreateEmployeeAsync(CreateUpdateEmployeeInput employeeData)
    {
        if (!await authorizationService.IsGrantedAsync(SmartHRPermissions.Employees.Create))
        {
            return "You are not authorized to create employee records";
        }

        try
        {
            var result = await employeeService.CreateAsync(employeeData);
            return JsonSerializer.Serialize(result);
        }
        catch (Exception ex)
        {
            return $"Error creating employee: {ex.Message}";
        }
    }
}
