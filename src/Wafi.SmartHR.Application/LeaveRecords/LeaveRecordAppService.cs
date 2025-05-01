using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq.Dynamic.Core;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
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
        var employeeQueryable = (await employeeRepository.GetQueryableAsync()).AsNoTracking();
        var leaveRCQueryable = (await leaveRecordRepository.GetQueryableAsync())
                            .AsNoTracking();

        var totalCount = await leaveRCQueryable.CountAsync();

        var result = await (from leave in leaveRCQueryable
                            join employee in employeeQueryable on leave.EmployeeId equals employee.Id into qo
                            from p in qo.DefaultIfEmpty()
                            select new LeaveRecordDto
                            {
                                Id = leave.Id,
                                EmployeeName = $"{p.FirstName} {p.LastName}",
                                StartDate = leave.StartDate,
                                EndDate = leave.EndDate,
                                Status = leave.Status,
                                Reason = leave.Reason,
                                TotalDays = leave.TotalDays
                            }).ToListAsync(); ;

        return result;
    }


    public async Task<PagedResultDto<LeaveRecordDto>> GetPagedListAsync(LeaveRecordFilter input)
    {
        string sortBy = !string.IsNullOrWhiteSpace(input.Sorting) ? input.Sorting : nameof(LeaveRecord.StartDate);

        var employeeQueryable = (await employeeRepository.GetQueryableAsync()).AsNoTracking();
        var leaveRCQueryable = (await leaveRecordRepository.GetQueryableAsync()).AsNoTracking();

        var totalCount = await leaveRCQueryable.CountAsync();

        var result = await (from leave in leaveRCQueryable
                            join employee in employeeQueryable on leave.EmployeeId equals employee.Id into qo
                            from p in qo.DefaultIfEmpty()
                            where string.IsNullOrEmpty(input.Filter) ||
                                         input.Filter.Contains(p.FirstName) ||
                                         input.Filter.Contains(p.PhoneNumber) ||
                                         input.Filter.Contains(p.Email) ||
                                         input.Filter.Contains(p.LastName)
                            select new LeaveRecordDto
                            {
                                Id = leave.Id,
                                EmployeeName = $"{p.FirstName} {p.LastName}",
                                StartDate = leave.StartDate,
                                EndDate = leave.EndDate,
                                Status = leave.Status,
                                Reason = leave.Reason,
                                TotalDays = leave.TotalDays
                            }).OrderBy(sortBy).PageBy(input).ToListAsync();

        return new PagedResultDto<LeaveRecordDto>(
            totalCount,
            result
        );
    }

    [Authorize(SmartHRPermissions.LeaveRecords.Create)]
    public async Task<LeaveRecordDto> CreateAsync(CreateUpdateLeaveRecordInput input)
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
    public async Task<LeaveRecordDto> UpdateStatusAsync(Guid id, UpdateLeaveStatusInput input)
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
