using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Wafi.SmartHR.Employees.Dtos;

namespace Wafi.SmartHR.Employees;

public interface IEmployeeAppService : IApplicationService
{
    Task<EmployeeDto> GetAsync(Guid id);
    Task<List<EmployeeDto>> GetListAsync();
    Task<EmployeeDto> CreateAsync(CreateUpdateEmployeeDto input);
    Task<EmployeeDto> UpdateAsync(Guid id, CreateUpdateEmployeeDto input);
    Task DeleteAsync(Guid id);
    Task<PagedResultDto<EmployeeDto>> GetPagedListAsync(EmployeeFilter filter);
}
