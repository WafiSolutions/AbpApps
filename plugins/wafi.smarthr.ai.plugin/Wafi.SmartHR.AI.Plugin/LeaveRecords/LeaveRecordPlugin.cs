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
using Wafi.SmartHR.LeaveRecords;
using Wafi.SmartHR.Permissions;

namespace Wafi.SmartHR.AI.Plugin.LeaveRecords
{
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
                return "You are not authorized to access leave records";
            }

            var result = await _leaveRecordService.GetListAsync();
            return JsonSerializer.Serialize(result);
        }
    }
}
