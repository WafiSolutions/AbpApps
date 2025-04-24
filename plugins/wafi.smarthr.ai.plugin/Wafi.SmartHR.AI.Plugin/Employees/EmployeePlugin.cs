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
using Wafi.SmartHR.Permissions;

namespace Wafi.SmartHR.AI.Plugin.Employees
{
    public class EmployeePlugin : ApplicationService, ITransientDependency
    {
        private readonly IEmployeeAppService _employeeService;
        private readonly IAuthorizationService _authorizationService;

        public EmployeePlugin(IEmployeeAppService employeeService, IAuthorizationService authorizationService) 
        {
            _employeeService = employeeService;
            _authorizationService = authorizationService;
        }

        [KernelFunction, Description("Get Channel Manager List")]
        public async Task<string> GetEmployeesAsync()
        {
            if (!await _authorizationService.IsGrantedAsync(SmartHRPermissions.Employees.Default))
            {
                return "You are not authorized to access employee records";
            }

            var result = await _employeeService.GetListAsync();
            return JsonSerializer.Serialize(result);
        }
    }
}
