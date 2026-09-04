using AutoMapper;
using PayrollSystem.API.DTOs.Attendance;
using PayrollSystem.API.DTOs.Employees;
using PayrollSystem.API.DTOs.Overtime;
using PayrollSystem.API.DTOs.WorkSchedules;
using PayrollSystem.API.Models;

namespace PayrollSystem.API.Mapping
{
    public class PayrollMappingProfile : Profile
    {
        public PayrollMappingProfile()
        {

            

            CreateMap<Attendance, AttendanceResponseDto>().ReverseMap();

            CreateMap<WorkSchedule, WorkScheduleResponseDto>().ReverseMap();

            CreateMap<Employee, EmployeeResponseDto>()
                .ForMember(
                    dest => dest.WorkScheduleName,
                    opt => opt.MapFrom(
                        src => src.WorkSchedule != null
                            ? src.WorkSchedule.Name
                            : null));

            CreateMap<OvertimeRequest, OvertimeResponseDto>()
                .ForMember(
                dest => dest.AttendanceDate,
                opt => opt.MapFrom(
                    src => src.Attendance.AttendanceDate
                    )
                )
                .ForMember(
                dest => dest.Status,
                opt => opt.MapFrom(
                    src => src.Status.ToString()
                    )
                );
        }
    }
}
