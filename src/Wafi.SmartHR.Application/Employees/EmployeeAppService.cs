using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Wafi.SmartHR.Employees.Dtos;
using System.Linq.Dynamic.Core;
using Wafi.SmartHR.Permissions;

namespace Wafi.SmartHR.Employees;

public class EmployeeAppService(IRepository<Employee, Guid> employeeRepository)
    : ApplicationService, IEmployeeAppService
{

    [Authorize(SmartHRPermissions.Employees.Default)]
    public async Task<EmployeeDto> GetAsync(Guid id)
    {
        var employee = await employeeRepository.GetAsync(id);
        return ObjectMapper.Map<Employee, EmployeeDto>(employee);
    }

    [Authorize(SmartHRPermissions.Employees.Default)]
    public async Task<List<EmployeeDto>> GetListAsync()
    {
        var employees = await employeeRepository.GetListAsync();
        return ObjectMapper.Map<List<Employee>, List<EmployeeDto>>(employees);
    }

    [Authorize(SmartHRPermissions.Employees.Default)]
    public async Task<PagedResultDto<EmployeeDto>> GetPagedListAsync(EmployeeFilter input)
    {
        string sortBy = !string.IsNullOrWhiteSpace(input.Sorting) ? input.Sorting : nameof(Employee.JoiningDate);

        var employeeQueryable = (await employeeRepository.GetQueryableAsync())
                                        .AsNoTracking()
                                        .WhereIf(!input.Filter.IsNullOrWhiteSpace(),
                                          e => e.FirstName.Contains(input.Filter) ||
                                               e.LastName.Contains(input.Filter) ||
                                               e.Email.Contains(input.Filter) ||
                                               e.PhoneNumber.Contains(input.Filter));

        var totalCount = await employeeQueryable.CountAsync();

        var result = await (from employee in employeeQueryable
                            select new EmployeeDto
                            {
                                Id = employee.Id,
                                FirstName = employee.FirstName,
                                LastName = employee.LastName,
                                DateOfBirth = employee.DateOfBirth,
                                Email = employee.Email,
                                PhoneNumber = employee.PhoneNumber,
                                JoiningDate = employee.JoiningDate,
                                TotalLeaveDays = employee.TotalLeaveDays,
                                RemainingLeaveDays = employee.RemainingLeaveDays,
                            }).OrderBy(sortBy).PageBy(input).ToListAsync();

        return new PagedResultDto<EmployeeDto>(
            totalCount,
            result
        );
    }

    [Authorize(SmartHRPermissions.Employees.Create)]
    public async Task<EmployeeDto> CreateAsync(CreateUpdateEmployeeInput input)
    {
        var employee = new Employee(
            GuidGenerator.Create(),
            input.FirstName,
            input.LastName,
            input.Email,
            input.PhoneNumber,
            input.DateOfBirth,
            input.JoiningDate,
            input.TotalLeaveDays
        );

        await employeeRepository.InsertAsync(employee);

        return ObjectMapper.Map<Employee, EmployeeDto>(employee);
    }

    [Authorize(SmartHRPermissions.Employees.Edit)]
    public async Task<EmployeeDto> UpdateAsync(Guid id, CreateUpdateEmployeeInput input)
    {
        var employee = await employeeRepository.GetAsync(id);

        employee.FirstName = input.FirstName;
        employee.LastName = input.LastName;
        employee.Email = input.Email;
        employee.PhoneNumber = input.PhoneNumber;
        employee.DateOfBirth = input.DateOfBirth;
        employee.JoiningDate = input.JoiningDate;
        employee.TotalLeaveDays = input.TotalLeaveDays;

        await employeeRepository.UpdateAsync(employee);

        return ObjectMapper.Map<Employee, EmployeeDto>(employee);
    }

    [Authorize(SmartHRPermissions.Employees.Delete)]
    public async Task DeleteAsync(Guid id)
    {
        await employeeRepository.DeleteAsync(id);
    }
}
