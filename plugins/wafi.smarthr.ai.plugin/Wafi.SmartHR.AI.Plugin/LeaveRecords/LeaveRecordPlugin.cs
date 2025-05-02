using System;
using System.ComponentModel;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.SemanticKernel;
using Volo.Abp.Application.Services;
using Volo.Abp.DependencyInjection;
using Wafi.SmartHR.LeaveRecords;
using Wafi.SmartHR.LeaveRecords.Dtos;
using Wafi.SmartHR.Permissions;

namespace Wafi.SmartHR.AI.Plugin.LeaveRecords;

public class LeaveRecordPlugin : ApplicationService, ITransientDependency
{
    private readonly ILeaveRecordAppService _leaveRecordService;
    private readonly IAuthorizationService _authorizationService;

    public LeaveRecordPlugin(ILeaveRecordAppService leaveRecordService, IAuthorizationService authorizationService)
    {
        _leaveRecordService = leaveRecordService;
        _authorizationService = authorizationService;
    }

    [KernelFunction, Description("Get leave record list")]
    public async Task<string> GetLeaveRecordslAsync()
    {
        if (!await _authorizationService.IsGrantedAsync(SmartHRPermissions.LeaveRecords.Default))
        {
            return "You are not authorized to access the leave records";
        }

        var result = await _leaveRecordService.GetListAsync();
        return JsonSerializer.Serialize(result);
    }



    [KernelFunction, Description("Get leave records of employee from their name, email, PhoneNumber")]
    public async Task<string> GetEmployeeLeaveRecordsAsync(string filter)
    {
        var employees = await _leaveRecordService.GetPagedListAsync(new LeaveRecordFilter() { Filter = filter });
        if (employees is null)
        {
            return $"leave records for filter '{filter}' not found.";
        }

        return JsonSerializer.Serialize(employees);
    }


    [KernelFunction, Description("Create a new leave record for a employee")]
    public async Task<string> CreateLeaveRecordAsync(CreateUpdateLeaveRecordInput leaveRecordData)
    {
        if (!await _authorizationService.IsGrantedAsync(SmartHRPermissions.LeaveRecords.Create))
        {
            return "You are not authorized to create leave record";
        }

        try
        {
            var result = await _leaveRecordService.CreateAsync(leaveRecordData);
            return JsonSerializer.Serialize(result);
        }
        catch (Exception ex)
        {
            return $"Error creating leave record: {ex.Message}";
        }
    }
}
