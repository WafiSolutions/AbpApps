using AutoMapper;
using Wafi.SmartHR.Employees;
using Wafi.SmartHR.Employees.Dtos;
using Wafi.SmartHR.LeaveRecords;
using Wafi.SmartHR.LeaveRecords.Dtos;

namespace Wafi.SmartHR
{
    public class WafiSmartHRAutoMapperProfile : Profile
    {
        public WafiSmartHRAutoMapperProfile()
        {
            CreateMap<Employee, EmployeeDto>();
            CreateMap<CreateUpdateEmployeeDto, Employee>();

            CreateMap<LeaveRecord, LeaveRecordDto>();
            CreateMap<CreateUpdateLeaveRecordDto, LeaveRecord>();
        }
    }
} 