using AutoMapper;
using Wafi.SmartHR.Employees;
using Wafi.SmartHR.Employees.Dtos;
using Wafi.SmartHR.LeaveRecords;
using Wafi.SmartHR.LeaveRecords.Dtos;

namespace Wafi.SmartHR;

public class SmartHRApplicationAutoMapperProfile : Profile
{
    public SmartHRApplicationAutoMapperProfile()
    {
        CreateMap<Employee, EmployeeDto>();
        CreateMap<CreateUpdateEmployeeInput, Employee>();

        CreateMap<LeaveRecord, LeaveRecordDto>();
        CreateMap<CreateUpdateLeaveRecordInput, LeaveRecord>();
    }
}
