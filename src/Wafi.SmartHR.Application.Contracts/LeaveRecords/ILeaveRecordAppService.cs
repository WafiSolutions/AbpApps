using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Wafi.SmartHR.LeaveRecords.Dtos;

namespace Wafi.SmartHR.LeaveRecords;

public interface ILeaveRecordAppService : IApplicationService
{
    Task<LeaveRecordDto> CreateAsync(CreateUpdateLeaveRecordDto input);
    Task<LeaveRecordDto> UpdateStatusAsync(Guid id, UpdateLeaveStatusDto input);
    Task<LeaveRecordDto> GetAsync(Guid id);
    Task<List<LeaveRecordDto>> GetListAsync();
    Task<PagedResultDto<LeaveRecordDto>> GetPagedListAsync(LeaveRecordFilter input);
    Task DeleteAsync(Guid id);
}
