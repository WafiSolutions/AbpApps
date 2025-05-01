using Microsoft.AspNetCore.Authorization;
using Microsoft.SemanticKernel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.DependencyInjection;
using Wafi.SmartHR.Employees;
using Wafi.SmartHR.Employees.Dtos;
using Wafi.SmartHR.Permissions;

namespace Wafi.SmartHR.AI.Plugin.Employees;

public class EmployeePlugin : ApplicationService, ITransientDependency
{
    private readonly IEmployeeAppService _employeeService;
    private readonly IAuthorizationService _authorizationService;

    public EmployeePlugin(IEmployeeAppService employeeService, IAuthorizationService authorizationService) 
    {
        _employeeService = employeeService;
        _authorizationService = authorizationService;
    }

    [KernelFunction, Description("Get employee list")]
    public async Task<string> GetEmployeesAsync()
    {
        if (!await _authorizationService.IsGrantedAsync(SmartHRPermissions.Employees.Default))
        {
            return "You are not authorized to access employee records";
        }

        var result = await _employeeService.GetListAsync();
        return JsonSerializer.Serialize(result);
    }


    [KernelFunction, Description("Create a new employee")]
    public async Task<string> CreateEmployeeAsync(string employeeData)
    {
        if (!await _authorizationService.IsGrantedAsync(SmartHRPermissions.Employees.Create))
        {
            return "You are not authorized to create employee records";
        }

        try
        {
            var employeeDto = JsonSerializer.Deserialize<CreateUpdateEmployeeInput>(employeeData);
            var result = await _employeeService.CreateAsync(employeeDto);
            return JsonSerializer.Serialize(result);
        }
        catch (Exception ex)
        {
            return $"Error creating employee: {ex.Message}";
        }
    }
}
