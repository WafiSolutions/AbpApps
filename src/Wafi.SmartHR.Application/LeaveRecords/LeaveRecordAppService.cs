using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Wafi.SmartHR.Employees;
using Wafi.SmartHR.LeaveRecords.Dtos;
using Wafi.SmartHR.Permissions;

namespace Wafi.SmartHR.LeaveRecords;

public class LeaveRecordAppService(
    IRepository<LeaveRecord, Guid> leaveRecordRepository,
    IRepository<Employee, Guid> employeeRepository) 
    : ApplicationService, ILeaveRecordAppService
{

    [Authorize(SmartHRPermissions.LeaveRecords.Default)]
    public async Task<LeaveRecordDto> GetAsync(Guid id)
    {
        var leaveRecord = await leaveRecordRepository.GetAsync(id);
        var employee = await employeeRepository.GetAsync(leaveRecord.EmployeeId);

        var dto = ObjectMapper.Map<LeaveRecord, LeaveRecordDto>(leaveRecord);
        dto.EmployeeName = $"{employee.FirstName} {employee.LastName}";

        return dto;
    }

    [Authorize(SmartHRPermissions.LeaveRecords.Default)]
    public async Task<List<LeaveRecordDto>> GetListAsync()
    {
        var leaveRecords = await leaveRecordRepository.GetListAsync();
        var employees = await employeeRepository.GetListAsync();

        var dtos = ObjectMapper.Map<List<LeaveRecord>, List<LeaveRecordDto>>(leaveRecords);

        foreach (var dto in dtos)
        {
            var employee = employees.FirstOrDefault(e => e.Id == dto.EmployeeId);
            if (employee != null)
            {
                dto.EmployeeName = $"{employee.FirstName} {employee.LastName}";
            }
        }

        return dtos;
    }

    [Authorize(SmartHRPermissions.LeaveRecords.Create)]
    public async Task<LeaveRecordDto> CreateAsync(CreateUpdateLeaveRecordDto input)
    {
        var employee = await employeeRepository.GetAsync(input.EmployeeId);

        var leaveRecord = new LeaveRecord(
            GuidGenerator.Create(),
            input.EmployeeId,
            input.StartDate,
            input.EndDate,
            input.Reason
        );

        await leaveRecordRepository.InsertAsync(leaveRecord);

        var dto = ObjectMapper.Map<LeaveRecord, LeaveRecordDto>(leaveRecord);
        dto.EmployeeName = $"{employee.FirstName} {employee.LastName}";

        return dto;
    }

    [Authorize(SmartHRPermissions.LeaveRecords.UpdateStatus)]
    public async Task<LeaveRecordDto> UpdateStatusAsync(Guid id, UpdateLeaveStatusDto input)
    {
        var leaveRecord = await leaveRecordRepository.GetAsync(id);
        var employee = await employeeRepository.GetAsync(leaveRecord.EmployeeId);

        if (input.Status == LeaveStatus.Approved && leaveRecord.Status != LeaveStatus.Approved)
        {
            employee.UpdateRemainingLeaveDays(leaveRecord.TotalDays);
            await employeeRepository.UpdateAsync(employee);
        }

        leaveRecord.UpdateStatus(input.Status);
        await leaveRecordRepository.UpdateAsync(leaveRecord);

        var dto = ObjectMapper.Map<LeaveRecord, LeaveRecordDto>(leaveRecord);
        dto.EmployeeName = $"{employee.FirstName} {employee.LastName}";

        return dto;
    }

    [Authorize(SmartHRPermissions.LeaveRecords.Delete)]
    public async Task DeleteAsync(Guid id)
    {
        await leaveRecordRepository.DeleteAsync(id);
    }
}
